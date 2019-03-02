using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DYFramework;
using log4net.Core;
using log4net.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mosaic.Api.Msg;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.Resolve;
using Mosaic.ServiceContracts;
using Newtonsoft.Json.Linq;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Invoice/[action]")]
    public class InvoiceController : Controller
    {
        private IInvoiceService invoiceService;
        private readonly IVirtualPrinterDataService virtualPrinterDataService;
        private readonly IDYLogService dYLogService;
        private readonly IRFIDBackupService rFIDBackupService;
        private readonly ILogger<InvoiceController> logger;
        private readonly IInvoiceUserInfoService invoiceUserInfoService;

        public class InvoiceResult
        {
            public bool Result { get; set; }
            public string Message { get; set; }
            public InvoiceDataObject Invoice { get; set; }
        }
        public InvoiceController(IInvoiceService invoiceService, 
                                 IVirtualPrinterDataService virtualPrinterDataService, 
                                 IDYLogService dYLogService, 
                                 IRFIDBackupService rFIDBackupService,
                                 ILogger<InvoiceController> logger, 
                                 IInvoiceUserInfoService invoiceUserInfoService
                                 )
        {
            this.invoiceService = invoiceService;
            this.virtualPrinterDataService = virtualPrinterDataService;
            this.dYLogService = dYLogService;
            this.rFIDBackupService = rFIDBackupService;
            this.logger = logger;
            this.invoiceUserInfoService = invoiceUserInfoService;
        }
        [HttpGet]
        public IList<InvoiceDataObject> GetInvoiceList()
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetList();
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetList,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = new InvoiceDataObject();
            try
            {
                result = this.invoiceService.Get(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/Get,错误信息：" + e.ToString() });
            }
            return new DyResult(result);
        }
        [HttpGet("{id}")]
        public DyResult GetByID(int id)
        {
            var result = new InvoiceDataObject();
            try
            {
                result = this.invoiceService.GetOne(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetOne,错误信息：" + e.ToString() });
            }
            return new DyResult(result);
        }
        [HttpPost]
        public InvoiceDataObject Update([FromBody]InvoiceDataObject invoice)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "更新invoice内容，invoiceid=" + invoice.ID + ",用户id=" + invoice.UserInfoID });
            var result = new InvoiceDataObject();
            try
            {
                result = this.invoiceService.Update(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/Update,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public InvoiceDataObject DeleteCode(int id)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "清除invoice内容，invoiceid=" + id + ",用户id=" });
            var result = new InvoiceDataObject();
            try
            {
                result = this.invoiceService.DeleteCode(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/DeleteCode,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public InvoiceDataObject Add()
        {
            Stream stream = HttpContext.Request.Body;
            byte[] buf = new byte[HttpContext.Request.ContentLength.Value];
            stream.Read(buf, 0, buf.Length);
            string str = Encoding.UTF8.GetString(buf);
            InvoiceDataObject jObject = JObject.Parse(str).ToObject<InvoiceDataObject>();
            return this.invoiceService.Add(jObject);
        }



        /// <summary>
        /// 虚拟打印机上传交货单PS文件及转换后的Txt文件
        /// </summary>
        /// <param name="printerData"></param>
        /// <returns>成功返回1,失败返回0,异常则输出异常信息</returns>
        public string UploadInvoice([FromBody]PrinterData printerData)
        {
            try
            {
                InvoiceDataObject invoice = InvoiceResolve.Resolve(printerData);
                if (invoice != null)
                {
                    invoice.CompanyID = printerData.CompanyID;
                    InvoiceDataObject result = null;
                    if (!this.invoiceService.Exists(invoice.No))
                    {
                        result = this.invoiceService.Add(invoice);
                    }
                    else
                    {
                        result = this.invoiceService.GetByNo(invoice.No);
                        invoice.CodeList = result.CodeList;
                        invoice.GroupNoList = result.GroupNoList;
                        invoice.Checked = result.Checked;
                        invoice.ErrGroupList = result.ErrGroupList;
                        invoice.ErrGroupNoList = result.ErrGroupNoList;
                        invoice.ErrRFIDList = result.ErrRFIDList;
                        invoice.UserInfoID = result.UserInfoID;
                        invoice.SubmitTime = result.SubmitTime;
                        invoice.Quantity = result.Quantity;
                        invoice.Memo = result.Memo;
                        invoice.ID = result.ID;
                        invoice.Flag = result.Flag;
                        result = this.Update(invoice);
                    }
                    VirtualPrinterDataDataObject prnData = new VirtualPrinterDataDataObject();
                    prnData.CompanyID = printerData.CompanyID;
                    prnData.PrintTime = printerData.PrintTime;
                    prnData.PSFileContent = printerData.PSFileContent;
                    prnData.TxtFileContent = printerData.TxtFileContent;
                    prnData.Flag = true;
                    this.virtualPrinterDataService.Add(prnData);
                    this.dYLogService.Add(new DYLogDataObject() { Memo = "打印交货单内容，invoiceid=" + result.ID + "，交货单号=" + result.No });
                    return result != null ? "1" : "0";
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/UploadInvoice,错误信息：" + ex.ToString() });
                return ex.ToString();
            }
        }

        [HttpPost]
        public Msg.DTOMessage<InvoiceDataObject> AddInvoice([FromBody]InvoiceDataObject invoiceDataObject)
        {
            var result = new InvoiceDataObject();
            //if (this.invoiceService.Exists(invoiceDataObject.No))
            //{
            //    result = this.invoiceService.GetByNo(invoiceDataObject.No);
            //    invoiceDataObject.CodeList = result.CodeList;
            //    invoiceDataObject.GroupNoList = result.GroupNoList;
            //    invoiceDataObject.ID = result.ID;
            //    invoiceDataObject.Flag = result.Flag;
            //    result = this.Update(invoiceDataObject);
            //    return new Msg.DTOMessage<InvoiceDataObject>() { Code = 2, Data = result };
            //}
            try
            {
                result = this.invoiceService.Add(invoiceDataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/Add,错误信息：" + e.ToString() });
            }
            return new Msg.DTOMessage<InvoiceDataObject>() { Code = 2, Data = result };
        }
        [HttpGet]
        public bool AddFlag(int id,int userInfoID)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "AddFlag发货单标记完成，invoiceid=" + id + ",操作人id=" + userInfoID });

            bool result = false;
            try
            {
                result = this.invoiceService.AddFlag(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/AddFlag,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public InvoiceResult AddFlagLimit(int id,int userInfoID)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "AddFlagLimit发货单标记完成，invoiceid=" + id +",操作人id="+userInfoID});
            string mess = "";
            bool result = false;
            try
            {
                result = this.invoiceService.AddFlagLimit(id,out string message);
                mess = message;
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/AddFlagLimit,错误信息：" + e.ToString() });
            }
            return new InvoiceResult() { Result=result,Message=mess};
        }
        [HttpGet]
        public IList<InvoiceDataObject> QueryFlagTrueByToday(int companyID)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.QueryFlagTrueByToday(companyID);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/QueryFlagTrueByToday,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<InvoiceDataObject> QueryFlagTrue(int companyID)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.QueryFlagTrue(companyID);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/QueryFlagTrue,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        //分公司查看invoice列表(1天）
        public IList<InvoiceDataObject> GetListByCompanyID(int id)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetListByCompanyID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListByCompanyID,错误信息：" + e.ToString() });
            }
            return result;
        }
        //分公司查看invoice列表（所有）
        public IList<InvoiceDataObject> GetList(int id)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetList(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetList,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public int Remove(int id)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "删除交货单，invoiceid=" + id });
            var result = 0;
            try
            {
                result = this.invoiceService.RemoveByID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/RemoveByID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<InvoiceDataObject> GetListNoGroup(int id)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetListNoGroup(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListNoGroup,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<InvoiceDataObject> GetListNoGroupSum(int id)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetListNoGroupSum(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListNoGroupSum,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<InvoiceDataObject> GetListGroup(int id)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetListGroup(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListGroup,错误信息：" + e.ToString() });
            }
            return result;
        }
        //更新二维码数据（关联分组）
        [HttpPost]
        public InvoiceResult UpdateQRCode([FromBody]InvoiceDataObject dataObject)
        {
            string message = "";
            var stopwatch = Stopwatch.StartNew();
            if (dataObject.ID == 0 || dataObject.CodeList == null || dataObject.CodeList.Count() == 0 || dataObject.CodeList.Count() > 1|| dataObject.UserInfoID==0)
                return new InvoiceResult();
            if(invoiceService.AlreadyShiped(dataObject.ID))
                return new InvoiceResult();
            string memo = "QRCODE关联发货单，invoiceid=" + dataObject.ID +
                           ",用户id=" + dataObject.UserInfoID +
                           ",codelist数据" + String.Join(",", dataObject.CodeList).ToString();
            if (this.dYLogService.ExistsMemo(memo))
                return new InvoiceResult();
            this.dYLogService.Add(
                new DYLogDataObject() {
                    Memo = memo
                });
            //this.rFIDBackupService.Add(new RFIDBackupDataObject() { CodeList = dataObject.CodeList, InvoiceID = dataObject.ID });
            //InvoiceUserInfoDataObject invoiceUserInfo = new InvoiceUserInfoDataObject() { InvoiceID=dataObject.ID,UserInfoID=dataObject.UserInfoID,CodeList=dataObject.CodeList };
            //bool flag=this.invoiceUserInfoService.UpdateQRCode(invoiceUserInfo);//备份人扫码
            this.logger.LogInformation($"接口UpdateQrcode：{memo}");
            var result = new InvoiceDataObject();
            try
            {
                result = this.invoiceService.UpdateQRCode(dataObject, out message);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/UpdateQRCode,错误信息：" + e.ToString() });
                message = e.ToString();
            }
            stopwatch.Stop();
            this.logger.LogInformation($"接口UpdateQrcode用时：{stopwatch.Elapsed.ToString()+",消息："+message}");
            return new InvoiceResult() { Invoice=result,Message=message};
        }
        [HttpPost]
        public InvoiceDataObject UpdateRFIDCode([FromBody]InvoiceDataObject dataObject)
        {
            if (dataObject.ID == 0 || dataObject.CodeList == null)
                return new InvoiceDataObject();
            this.dYLogService.Add(new DYLogDataObject() { Memo = "RFID关联发货单，invoiceid=" + dataObject.ID + ",用户id=" + dataObject.UserInfoID + ",codelist数据" + String.Join(",", dataObject.CodeList).ToString() });
            this.rFIDBackupService.Add(new RFIDBackupDataObject() { CodeList = dataObject.CodeList, InvoiceID = dataObject.ID });
            var result = new InvoiceDataObject();
            try
            {
                result = this.invoiceService.UpdateRFIDCode(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/UpdateRFIDCode,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public bool RemoveCode([FromBody]InvoiceDataObject dataObject)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "取消关联发货单，invoiceid=" + dataObject.ID + ",用户id=" + dataObject.UserInfoID });
            var result = false;
            try
            {
                result = this.invoiceService.RemoveCode(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/RemoveCode,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<InvoiceDataObject> GetInvoices([FromBody]InvoiceDataObject dataObject)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetInvoices(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetInvoices,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public InvoiceDataObject RelationCode(int id, int rfid)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "重新关联发货单，交货单id=" + id + ",空单i=" + rfid });
            var result = new InvoiceDataObject();
            try
            {
                result = this.invoiceService.RelationCode(id, rfid);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/RelationCode,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<InvoiceDataObject> Query(string query)
        {
            if (String.IsNullOrEmpty(query))
                return new List<InvoiceDataObject>();
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.Query(query);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/Query,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<CategoryDataObject> GetCategoryByCode([FromBody]InvoiceDataObject dataObject)
        {
            IList<CategoryDataObject> result = new List<CategoryDataObject>();
            try
            {
                result = this.invoiceService.GetCategoryByCode(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetCategoryByCode,错误信息：" + e.ToString() });
            }
            return result;
        }

        [HttpPost]
        public IList<CategoryDataObject> GetCategoryByRFID([FromBody]InvoiceDataObject dataObject)
        {
            IList<CategoryDataObject> result = new List<CategoryDataObject>();
            try
            {
                result = this.invoiceService.GetCategoryByRFID(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetCategoryByRFID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<GroupDataObject> GetGroupByCode([FromBody]InvoiceDataObject dataObject)
        {
            IList<GroupDataObject> result = new List<GroupDataObject>();
            try
            {
                result = this.invoiceService.GetGroupByCode(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetGroupByCode,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public DTOMessage<IList<GroupDataObject>> GetGroupByID([FromBody]InvoiceDataObject dataObject)
        {
            IList<GroupDataObject> result = new List<GroupDataObject>();
            try
            {
                result = this.invoiceService.GetGroupByID(dataObject, out string quantity);
                return new DTOMessage<IList<GroupDataObject>> { Data = result, Quantity = quantity };
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetGroupByID,错误信息：" + e.ToString() });
                return new DTOMessage<IList<GroupDataObject>> { Data = result };
            }
            //return result;
        }
        [HttpPost]
        public IList<GroupDataObject> GetGroupByRFID([FromBody]InvoiceDataObject dataObject)
        {
            IList<GroupDataObject> result = new List<GroupDataObject>();
            try
            {
                result = this.invoiceService.GetGroupByRFID(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetGroupByRFID,错误信息：" + e.ToString() });
            }
            return result;
        }

        [HttpGet]
        public IList<int> GetSumList(int id, int userInfoID)
        {
            IList<int> result = new List<int>();
            try
            {
                result = this.invoiceService.GetSumList(id, userInfoID);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetSumList,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<int> GetSum(int id, int userInfoID)
        {
            IList<int> result = new List<int>();
            try
            {
                result = this.invoiceService.GetSum(id, userInfoID);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetSum,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<GroupDataObject> GetGroupByNo([FromBody]InvoiceDataObject dataObject)
        {
            IList<GroupDataObject> result = new List<GroupDataObject>();
            try
            {
                result = this.invoiceService.GetGroupByNo(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetGroupByNo,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public InvoiceDataObject GetInfomation(int id)
        {
            InvoiceDataObject invoice = new InvoiceDataObject();
            try
            {
                invoice = this.invoiceService.GetInfomation(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetInfomation,错误信息：" + e.ToString() });
            }
            return invoice;
        }
        [HttpGet]
        public InvoiceDataObject UpdateErrNo(int id, int groupID)
        {
            InvoiceDataObject invoice = new InvoiceDataObject();
            try
            {
                invoice = this.invoiceService.UpdateErrNo(id, groupID);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/UpdateGroupNo,错误信息：" + e.ToString() });
            }
            return invoice;
        }
        [HttpGet]
        public InvoiceDataObject UpdateRightNo(int id, int groupID)
        {
            InvoiceDataObject invoice = new InvoiceDataObject();
            try
            {
                invoice = this.invoiceService.UpdateRightNo(id, groupID);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/UpdateGroupNo,错误信息：" + e.ToString() });
            }
            return invoice;
        }
        [HttpPost]
        public bool AddUserInfo([FromBody]InvoiceDataObject invoice)
        {
            bool result = false;
            try
            {
                result = this.invoiceService.AddUserInfo(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/AddUserInfo,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public bool RemoveUserInfo([FromBody]InvoiceDataObject invoice)
        {
            bool result = false;
            try
            {
                result = this.invoiceService.RemoveUserInfo(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/AddUserInfo,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<InvoiceDataObject> GetListByUserInfoID([FromBody]InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> list = new List<InvoiceDataObject>();
            try
            {
                list = this.invoiceService.GetListByUserInfoID(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListByUserInfoID,错误信息：" + e.ToString() });
            }
            return list;
        }
        [HttpPost]
        public IList<InvoiceDataObject> GetListGroupByUserInfoID([FromBody]InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetListGroupByUserInfoID(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListGroupByUserInfoID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<InvoiceDataObject> QueryFlagTrueByUserInfoID([FromBody]InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.QueryFlagTrueByUserInfoID(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/QueryFlagTrueByUserInfoID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<InvoiceDataObject> QueryFlagTrueTodayByUserInfoID([FromBody]InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.QueryFlagTrueTodayByUserInfoID(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/QueryFlagTrueTodayByUserInfoID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<InvoiceDataObject> GetListNoGroupByUserInfoID([FromBody]InvoiceDataObject invoice)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetListNoGroupByUserInfoID(invoice);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListNoGroupByUserInfoID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public InvoiceDataObject QuantitySum([FromBody]InvoiceDataObject dataObject)
        {
            InvoiceDataObject result = new InvoiceDataObject();
            if (dataObject.Memo == null || dataObject.ID == 0)
                return result;
            try
            {
                result = this.invoiceService.QuantitySum(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/QuantitySum,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public InvoiceResult TransferGroupNo([FromBody]InvoiceDataObject dataObject)
        {
            bool result = false;
            string mess = "";
            if (dataObject == null || dataObject.ID == 0 || dataObject.UserInfoID == 0)
                return new InvoiceResult() { Result = result };
            if (dataObject.NewID != 0 && dataObject.ID != 0)
            {
                try
                {
                    this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/TransferGroupNo,操作人id：" + dataObject.UserInfoID + "，原单号：" + dataObject.ID + "转移单号至：" + dataObject.NewID });
                    result = this.invoiceService.TransferGNo(dataObject, out string message);
                    mess = message;
                }
                catch (Exception e)
                {
                    this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/TransferGroupNo,错误信息：" + e.ToString() });
                }
            }
            else
            {
                try
                {
                    this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/RevokeGNo,操作人id：" + dataObject.UserInfoID + "，原单号：" + dataObject.ID });
                    result = this.invoiceService.RevokeGNo(dataObject, out string message);
                    mess = message;
                    return new InvoiceResult() { Result = result, Message = mess };
                }
                catch (Exception e)
                {
                    this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/RevokeGNo,错误信息：" + e.ToString() });
                }
            }
            return new InvoiceResult() { Result = result, Message = mess };
        }
        [HttpPost]
        public InvoiceResult RevokeQRCode([FromBody]InvoiceDataObject dataObject)
        {
            bool result = false;
            string mess = "";
            if (dataObject == null || string.IsNullOrWhiteSpace(dataObject.RevokeQRCode) || dataObject.ID == 0 || dataObject.UserInfoID == 0)
                return new InvoiceResult() { Result = result };
            try
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/RevokeQRCode,操作人id：" + dataObject.UserInfoID + "，原单号：" + dataObject.ID + "，撤销二维码：" + dataObject.RevokeQRCode });
                result = this.invoiceService.RevokeQRCode(dataObject, out string message);
                mess = message;
                return new InvoiceResult() { Result = result, Message = mess };
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/RevokeQRCode,错误信息：" + e.ToString() });
            }
            return new InvoiceResult() { Result = result, Message = mess };
        }
        [HttpPost]
        public InvoiceResult UpdateFlag([FromBody]InvoiceDataObject dataObject)
        {
            bool result = false;
            string mess = "";
            if (dataObject == null || dataObject.ID == 0 || dataObject.UserInfoID == 0)
                return new InvoiceResult() { Result = result };
            try
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/UpdateFlag,操作人id：" + dataObject.UserInfoID + "，单号：" + dataObject.ID  });
                result = this.invoiceService.UpdateFlag(dataObject, out string message);
                mess = message;
                return new InvoiceResult() { Result = result, Message = mess };
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/UpdateFlag,错误信息：" + e.ToString() });
            }
            return new InvoiceResult() { Result = result, Message = mess };
        }
        [HttpPost]
        public IList<InvoiceDataObject> GetListByQuery([FromBody]InvoiceDataObject dataObject)
        {
            return this.invoiceService.GetListByQuery(dataObject);
        }
        [HttpGet]
        public InvoiceResult DeleteCodeByUserInfoID(int id, int userInfoID)
        {
            string mess = null;
            InvoiceDataObject result = new InvoiceDataObject();
            try
            {
                 result= this.invoiceService.DeleteCodeByUserInfoID(id, userInfoID,out string message);
                 mess = message;
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/DeleteCodeByUserInfoID,错误信息：" + e.ToString() });
                mess = e.ToString();
            }
            return new InvoiceResult() { Invoice = result, Message = mess };
        }
        [HttpPost]
        public IList<InvoiceDataObject> QueryShip([FromBody]InvoiceDataObject dataObject)
        {
            string mess = null;
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.QueryShip(dataObject);
                
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/QueryShip,错误信息：" + e.ToString() });
                mess = e.ToString();
            }
            return result;
        }
        [HttpGet]
        public   IList<InvoiceDataObject> GetListGroupNoUser(int id)
        {
            IList<InvoiceDataObject> result = new List<InvoiceDataObject>();
            try
            {
                result = this.invoiceService.GetListGroupNoUser(id);

            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/GetListGroupNoUser,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<InvoiceShipmentDataObject> TotalDate([FromBody]InvoiceDataObject dataObject)
        {
            IList<InvoiceShipmentDataObject> result = new List<InvoiceShipmentDataObject>();
            try
            {
                result = this.invoiceService.TotalDate(dataObject);

            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoice/TotalDate,错误信息：" + e.ToString() });
            }
            return result;
        }
    }
}