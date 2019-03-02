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
    [Route("api/InvoiceUserInfo/[action]")]
    public class InvoiceUserInfoController : Controller
    {
        private IInvoiceUserInfoService invoiceUserInfoService;
        private readonly IVirtualPrinterDataService virtualPrinterDataService;
        private readonly IDYLogService dYLogService;
        private readonly IRFIDBackupService rFIDBackupService;
        private readonly ILogger<InvoiceUserInfoController> logger;

        public class InvoiceUserInfoResult
        {
            public bool Result { get; set; }
            public string Message { get; set; }
        }
        public InvoiceUserInfoController(IInvoiceUserInfoService invoiceUserInfoService, 
                                 IVirtualPrinterDataService virtualPrinterDataService, 
                                 IDYLogService dYLogService, 
                                 IRFIDBackupService rFIDBackupService,
                                 ILogger<InvoiceUserInfoController> logger
                                 )
        {
            this.invoiceUserInfoService = invoiceUserInfoService;
            this.logger = logger;
        }
        [HttpGet]
        public IList<InvoiceUserInfoDataObject> GetInvoiceUserInfoList()
        {
            IList<InvoiceUserInfoDataObject> result = new List<InvoiceUserInfoDataObject>();
            try
            {
                result = this.invoiceUserInfoService.GetList();
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoiceUserInfo/GetList,错误信息：" + e.ToString() });
            }
            return result;
        }
      
      
        [HttpPost]
        public InvoiceUserInfoDataObject Update([FromBody]InvoiceUserInfoDataObject invoiceUserInfo)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "更新invoiceUserInfo内容，invoiceUserInfoid=" + invoiceUserInfo.ID + ",用户id=" + invoiceUserInfo.UserInfoID });
            var result = new InvoiceUserInfoDataObject();
            try
            {
                result = this.invoiceUserInfoService.Update(invoiceUserInfo);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：invoiceUserInfo/Update,错误信息：" + e.ToString() });
            }
            return result;
        }
      

    }
}