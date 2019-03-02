using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mosaic.Resolve
{
    public static class InvoiceResolve
    {
        private static readonly string[] PROVINCE_SHORT =
{
            "京","津","沪","渝","冀",
            "豫","云","辽","黑","湘",
            "皖","鲁","新","苏","浙",
            "赣","鄂","桂","甘","晋",
            "蒙","陕","吉","闽","贵",
            "粤","青","藏","川","宁",
            "琼","港","澳","台"
        };
        private static string[] PLATENO_PREFIX;
        static InvoiceResolve()
        {
            List<string> result = new List<string>();
            foreach (var str in PROVINCE_SHORT)
            {
                for (char ch = 'A'; ch <= 'Z'; ch++)
                {
                    result.Add(str + ch);
                }
            }
            PLATENO_PREFIX = result.ToArray();
        }
        public static InvoiceDataObject Resolve(PrinterData printerData)
        {
            try
            {
                printerData = convertPSFile(printerData);
            if (!printerData.TxtFileContent.Contains("交货单"))
                return null;

                InvoiceDataObject invoice = new InvoiceDataObject();

                string txt = printerData.TxtFileContent;
                string[] strArr = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None)
                                    .Where(p => !string.IsNullOrWhiteSpace(p)&&p.Trim()!="REPRINT")
                                    .Select(p=>p.Trim()).ToArray();

                //var carStrings = strArr[25]=="检验批次号"?strArr[31]:strArr[32];


                TruckInfo truck = null;
                var arrived = strArr.Find("到站");
                var strGray = strArr.Find("具体");
                if (arrived > 0)
                {

                    string trainStr = string.Empty;
                    for (int i = arrived; i < strGray; i++)
                        trainStr += strArr[i];
                    truck = getTrain(trainStr);
                    invoice.DriverName = truck.Driver;
                    invoice.DriverPhoneNo = truck.DriverPhoneNo;
                    invoice.PlateNumber = truck.PlateNo;

                }
                var countryStrArry = strArr[15].Split(" ").Distinct().Where(p=>!string.IsNullOrWhiteSpace(p)).ToArray();
                var countryStr = string.Join(" ",countryStrArry.Take(countryStrArry.Length-1));
                invoice.CompanyAddress = $"{strArr[4]} {strArr[5]} {strArr[7]} {strArr[10]} {strArr[13]} {countryStr}";
                //                string[] info = strArr.Length > 9 ? strArr[9].Split(' ') : new string[] { };
                invoice.No = strArr[8];
                invoice.InvoiceTime = strArr[11];
                invoice.CustomerOrderNo = strArr[14];
                invoice.CustomerOrderTime = strArr[16];
                invoice.OrderNo = strArr[18];
                invoice.OrderTime = strArr[20];
                invoice.CustomerNo = strArr[22].Split(" ").Distinct().Where(p => !string.IsNullOrWhiteSpace(p)).Last();
                invoice.BatchNumber = strArr[25] == "检验批次号" ? "" : strArr[25];


                string[] dealerInfo = strArr.Length > 6 ? strArr[6].Split(' ') : new string[] { };

                invoice.DealerName = strArr[22].Split(" ")[0];
                invoice.DealerPostcord = strArr[24].Split(" ").FirstOrDefault();
                invoice.DealerPlace = strArr[24].Split(" ").LastOrDefault();


                invoice.ShipmentMode = strArr[25] == "检验批次号" ? strArr[27] : strArr[28];
                invoice.DeliveryMode = strArr[25] == "检验批次号" ? strArr[29] : strArr[30];



                List<string> strList = new List<string>();
                List<string> qtyList = new List<string>();
                List<string> categoryList = new List<string>();
                List<string> projectList = new List<string>();
                List<string> categoryNoList = new List<string>();

                int skips = Array.IndexOf(strArr, "具体装运信息") + 1;

                foreach (var item in strArr.Skip(skips))
                {
                    if(truck==null&&(PLATENO_PREFIX.Any(p=>item.Contains(p))||item.Contains("到站")))
                    {
                        try
                        {
                            if (item.Contains("到站"))
                                truck = getTrain(item);
                            else
                            {
                                var str = item;
                                if (str.Contains(":"))
                                    str = str.Split(":")[1];
                                if (str.Contains("："))
                                    str = str.Split("：")[1];
                                truck = getTruck(new List<string> { str });
                            }


                            invoice.DriverName = truck.Driver;
                            invoice.DriverPhoneNo = truck.DriverPhoneNo;
                            invoice.PlateNumber = truck.PlateNo;
                        }
                        catch { }
                    }

                    if (item.Contains("MTN"))
                    {
                        var arr = item.Split(" ").Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
                        arr = arr.Take(arr.Length - 1).ToArray();
                        strList.AddRange(arr);
                    }

                    if (item.Contains("BB") || item.Contains("MOS"))
                        categoryList.Add(item);
                }
                projectList = strList.Where(p => p.StartsWith("0")).ToList();
                categoryNoList = strList.Where(p => !p.StartsWith("0")&&!p.Contains(".")).ToList();
                qtyList.AddRange(strList.Where(p => p.Contains(".")));

                List<InvoiceShipmentDataObject> details = new List<InvoiceShipmentDataObject>();
                for (int i = 0; i < projectList.Count; i++)
                {
                    try
                    {
                        InvoiceShipmentDataObject detail = new InvoiceShipmentDataObject();
                        detail.Project = projectList[i];
                        detail.MaterialNo = categoryNoList[i];
                        detail.Describe = categoryList[i];
                        detail.Quantity = Convert.ToDouble(qtyList[i]);
                        details.Add(detail);
                    }
                    catch { }
                }

                invoice.InvoiceShipmentList = details;

                return invoice;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private static TruckInfo getTrain(string item)
        {
            List<string> sections = new List<string>();
            var strArr = item.ToArray();
            while (strArr.Length > 0)
            {
                int i = 0;
                for(; i < strArr.Length; i++)
                {
                    if (strArr[i] == ',' || strArr[i] == '，')
                        break;
                }
                string str = string.Join("", strArr.Take(i));
                sections.Add(str);
                strArr = strArr.Skip(i+1).ToArray();
            }
            Dictionary<string, string> info = new Dictionary<string, string>();
            foreach(var str in sections)
            {
                string[] arr;
                if (str.Contains(":"))
                    arr = str.Split(":");
                else
                    arr = str.Split("：");
                if (arr.Length > 1)
                    info.Add(arr[0], arr[1]);
            }
            TruckInfo truck = new TruckInfo();
            foreach(var key in info.Keys)
            {
                if (key.Contains("到站"))
                    truck.PlateNo = info[key];
                if (key.Contains("联系人")|| key.Contains("提货人"))
                    truck.Driver = info[key];
                if (key.Contains("电话"))
                    truck.DriverPhoneNo = info[key].Substring(0, 11);
                
            };
            return truck;
        }

        private static PrinterData convertPSFile(PrinterData printerData)
        {
            string tempPsFile =Path.GetTempFileName();
            string tempTxtFile = Path.GetTempFileName();
            using (FileStream fs = new FileStream(tempPsFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buf = Encoding.UTF8.GetBytes(printerData.PSFileContent);
                fs.Write(buf, 0, buf.Length);
            }
            ConvertPsToTxt(tempPsFile, tempTxtFile);
            string content = string.Empty;
            using(FileStream fs=new FileStream(tempTxtFile, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                content = reader.ReadToEnd();
            }
            printerData.TxtFileContent = content;
            File.Delete(tempPsFile);
            File.Delete(tempTxtFile);
            return printerData;
        }

        private static TruckInfo getTruck(List<string> carStrings)
        {
            Dictionary<string, TruckInfo> truckDict = new Dictionary<string, TruckInfo>();
            foreach (var str in carStrings)
            {
                List<string> temp = new List<string>();
                char[] arr = str.ToArray();
                while (arr.Length > 0)
                {
                    int i = 0;
                    for (; i < arr.Length; i++)
                    {
                        if (arr[i] == ',' || arr[i] == '，'||arr[i]==':'||arr[i]=='：')
                            break;
                    }
                    string section = string.Join("", arr.Take(i).ToArray());
                    temp.Add(section);
                    arr = arr.Skip(i + 1).ToArray();
                }
                TruckInfo truck = new TruckInfo();
                
                foreach (var sec in temp)
                {
                    string prefix = sec.Substring(0, 2).ToUpper();
                    var telStr = sec.Split(' ')[0];
                    long tel = 0;
                    Regex r = new Regex(@"^\d+$");
                    if (PLATENO_PREFIX.Contains(prefix))
                        truck.PlateNo = string.Join("", sec.Split(' '));
                    else
                    if (Int64.TryParse(telStr, out tel) && telStr.Length > 6)
                        truck.DriverPhoneNo = telStr;
                    else if (!r.IsMatch(sec)&&!containsNumber(sec))
                    {
                        truck.Driver = sec;
                    }
                }
                    if (!truckDict.ContainsKey(truck.PlateNo))
                        truckDict.Add(truck.PlateNo, truck);



            }
            return truckDict.Values.FirstOrDefault();
        }

        private static bool containsNumber(string sec)
        {
            char[] num = new char[] { '0', '1', '2', '3', '4', '5','6','7','8','9' };
            foreach(var item in sec)
            {
                if (num.Contains(item))
                    return true;
            }
            return false;
        }

        private static void ConvertPsToTxt(string psFilename, string txtFilename)
        {
            var command = Environment.CurrentDirectory + "\\ps2txt.exe";
            var args = string.Format("\"{0}\" \"{1}\"", psFilename, txtFilename);
            var pro = Process.Start(command, args);
            pro.WaitForExit();
        }
    }
    class TruckInfo
    {
        public string PlateNo { get; set; }
        public string Driver { get; set; }
        public string DriverPhoneNo { get; set; }
    }
}
