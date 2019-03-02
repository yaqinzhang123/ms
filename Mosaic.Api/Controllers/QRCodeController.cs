using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Api.Msg;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/QRCode/[action]")]
    public class QRCodeController : Controller
    {
        private IQRCodeService qRCodeService;
        private IOperationService operationService;
        private readonly IDYLogService dYLogService;

        public QRCodeController(IQRCodeService qRCodeService,IOperationService operationService,IDYLogService dYLogService)
        {
            this.qRCodeService = qRCodeService;
            this.operationService = operationService;
            this.dYLogService = dYLogService;
        }
        [HttpGet]
        public IList<QRCodeDataObject> GetQRCodeList()
        {
            IList<QRCodeDataObject> result = new List<QRCodeDataObject>();
            try
            {
                result = this.qRCodeService.GetQRCodeList();
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetQRCodeList,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<QRCodeDataObject> GetListByProductionID(int id)
        {
            IList<QRCodeDataObject> result = new List<QRCodeDataObject>();
            try
            {
                result = this.qRCodeService.GetListByProductionID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetListByProductionID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public QRCodeDataObject GetEndQRByProductionLineID(int productionLineID)
        {
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.GetEndQRByProductionLineID(productionLineID);

            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetEndQRByProductionLineID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.GetByID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetByID,错误信息：" + e.ToString() });
            }
            return new DyResult(result);
        }
        [HttpPost]
        public QRCodeDataObject Update([FromBody] QRCodeDataObject qRCode)
        {

            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.Update(qRCode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/Update,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public QRCodeDataObject Add(QRCodeDataObject qRCode)
        {
            //OperationDataObject operation = this.operationService.GetOperationByProductionLine(qRCode.ProductionLineID);
            //if (!operation.State)
            //    return new QRCodeDataObject();
            try
            {
                qRCode.Content = Encoding.UTF8.GetString(Convert.FromBase64String(qRCode.Content)).Trim();
            }
            catch
            {
            }
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.Add(qRCode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/Add,错误信息：" + e.ToString() });
            }
            return result;
        }

        [HttpPost]
        public int AddList([FromBody]IList<QRCodeDataObject> qRCodeList)
        {
            int result = 0;
            try
            {
                result = this.qRCodeService.AddList(qRCodeList);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/AddList,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public int Remove(int id)
        {
            int result = 0;
            try
            {
                result = this.qRCodeService.RemoveByID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/RemoveByID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<QRCodeDataObject> Query(string qrcode)
        {
            IList<QRCodeDataObject> result = new List<QRCodeDataObject>();
            try
            {
                result = this.qRCodeService.Query(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/Query,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public QRCodeDataObject GetContent([FromBody]QRCodeDataObject qrcode)
        {
            return this.qRCodeService.GetContent(qrcode.Content.Trim());
        }
        [HttpPost]
        public IList<QRCodeDataObject> GetListByTime([FromBody]QRCodeDataObject qrcode)
        {
            IList<QRCodeDataObject> result = new List<QRCodeDataObject>();
            try
            {
                result = this.qRCodeService.GetListByTime(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetListByTime,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public IList<QRCodeDataObject> GetListByMinute(int productionLineID,int minute)
        {
            QRCodeDataObject qrcode = new QRCodeDataObject();
            qrcode.ProductionLineID = productionLineID;
            qrcode.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            qrcode.CreateTime = DateTime.Now.AddMinutes(-minute).ToString("yyyy-MM-dd HH:mm:ss");
            IList<QRCodeDataObject> result = new List<QRCodeDataObject>();
            try
            {
                result = this.qRCodeService.GetListByTime(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetListByMinute,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<QRCodeDataObject> UpdateList([FromBody]IList<QRCodeDataObject> qrList)
        {
            IList<QRCodeDataObject> result = new List<QRCodeDataObject>();
            try
            {
                result = this.qRCodeService.UpdateList(qrList);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/UpdateList,错误信息：" + e.ToString() });
            }
            return result;
            
        }
        [HttpPost]
        public IList<QRCodeDataObject> Unlock([FromBody]QRCodeDataObject qr)
        {
            IList<QRCodeDataObject> result = new List<QRCodeDataObject>();
            try
            {
                result = this.qRCodeService.Unlock(qr);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/Unlock,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public QRCodeDataObject UpdateSingle([FromBody]QRCodeDataObject qrcode)
        {
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.UpdateSingle(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/UpdateSingle,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public QRCodeDataObject UpdateStartRoot([FromBody]QRCodeDataObject qrcode)
        {
            if (qrcode == null)
                return null;
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.UpdateStartRoot(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/UpdateStartRoot,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public QRCodeDataObject UpdateManualSkip([FromBody]QRCodeDataObject qrcode)
        {
            if (qrcode == null)
                return null;
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.UpdateManualSkip(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/UpdateManualSkip,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public QRCodeDataObject UpdateVirtualAdd([FromBody]QRCodeDataObject qrcode)
        {
            if (qrcode == null)
                return null;
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.UpdateVirtualAdd(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/UpdateVirtualAdd,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public QRCodeDataObject UpdateEndRoot([FromBody]QRCodeDataObject qrcode)
        {
            if (qrcode == null)
                return null;
            QRCodeDataObject result = new QRCodeDataObject();
            try
            {
                result = this.qRCodeService.UpdateEndRoot(qrcode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/UpdateEndRoot,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public bool AddEndRoot(int productionLineID)
        {
            bool result = false;
            try
            {
                QRCodeDataObject endqr = this.GetEndQRByProductionLineID(productionLineID);
                endqr.EndRoot = true;
                QRCodeDataObject qr = this.qRCodeService.UpdateEndRoot(endqr);
                result = qr.EndRoot == true ? true : false;
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/AddEndRoot,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public bool AddEnd([FromBody]QRCodeDataObject qrcode)
        {
            bool result = false;
            try
            {
                QRCodeDataObject endqr = this.qRCodeService.GetByContent(qrcode);
                if (endqr == null)
                    return false;
                endqr.EndRoot = true;
                QRCodeDataObject qr = this.qRCodeService.UpdateEndRoot(endqr);
                result = qr.EndRoot == true ? true : false;
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/AddEnd,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public bool IsThree(string content)
        {
            return this.qRCodeService.IsThree(content);
        }
        [HttpPost]
        public IList<ReadRateQRCode> GetReadRate([FromBody]QRCodeDataObject dataObject)
        {
            IList<ReadRateQRCode> result = new List<ReadRateQRCode>();
            try
            {
                if (dataObject == null)
                    return null;
                result = this.qRCodeService.GetReadRate(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetReadRate,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<TotalQRCode> GetTotal([FromBody]QRCodeDataObject dataObject)
        {
            IList<TotalQRCode> result = new List<TotalQRCode>();
            try
            {
                if (dataObject == null)
                    return null;
                result = this.qRCodeService.GetTotal(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetTotal,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<CategoryDataObject> GetCategoryByTime([FromBody]QRCodeDataObject dataObject)
        {
            IList<CategoryDataObject> result = new List<CategoryDataObject>();
            try
            {
                if (dataObject == null)
                    return null;
                result= this.qRCodeService.GetCategoryByTime(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetCategoryByTime,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<ReadRateQRCode> GetRateAndTotal([FromBody]QRCodeDataObject dataObject)
        {
            IList<ReadRateQRCode> result = new List<ReadRateQRCode>();
            try
            {
                if (dataObject == null)
                    return null;
                result = this.qRCodeService.GetRateAndTotal(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/GetRateAndTotal,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<ReadRateQRCode> TotalDate([FromBody]QRCodeDataObject dataObject)
        {
            IList<ReadRateQRCode> result = new List<ReadRateQRCode>();
            try
            {
                if (dataObject == null)
                    return null;
                result = this.qRCodeService.TotalDate(dataObject);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：qrcode/TotalDate,错误信息：" + e.ToString() });
            }
            return result;
        }
    }
}