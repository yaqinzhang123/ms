using Mosaic.Resolve;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mosaic.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"E:\MosaicPostScriptFiles";
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo result = dir.CreateSubdirectory("Result");


            string psFile = result.FullName + "\\PS_026.ps";
            string txtFile = result.FullName + "\\TXT_026.txt";

            //FileInfo[] fileInfos = dir.GetFiles();
            //int i = 1;
            //foreach(var file in fileInfos)
            //{
                
            //    var psFileName = result.FullName + $"\\PS_{i.ToString("d3")}.ps";
            //    var txtFileName = result.FullName + $"\\TXT_{i.ToString("d3")}.txt";
            //    file.CopyTo(psFileName,true);
            //    ConvertPsToTxt(psFileName, txtFileName);
            //    i++;
            //}

            PrinterData printerData = new PrinterData
            {
                CompanyID = 1,
                ID = 1,
                PrintTime = DateTime.Now,
                PSFileContent=File.ReadAllText(psFile),
                TxtFileContent=File.ReadAllText(txtFile)
            };
            var invoice = InvoiceResolve.Resolve(printerData);
            string jsonStr = JObject.FromObject(invoice).ToString();
            Console.WriteLine(jsonStr);
            Console.ReadLine();
        }
        private static void ConvertPsToTxt(string psFilename, string txtFilename)
        {
            var command = Environment.CurrentDirectory + "\\ps2txt.exe";
            var args = string.Format("\"{0}\" \"{1}\"", psFilename, txtFilename);
            Process.Start(command, args);
        }
    }
}
