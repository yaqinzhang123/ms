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
    [Route("api/RelationRFIDQRCode/[action]")]
    public class RelationRFIDQRCodeController : Controller
    {
        private IRelationRFIDQRCodeService relationRFIDQRCodeService;
        private readonly IDYLogService dYLogService;

        public RelationRFIDQRCodeController(IRelationRFIDQRCodeService relationRFIDQRCodeService, IDYLogService dYLogService)
        {
            this.relationRFIDQRCodeService = relationRFIDQRCodeService;
            this.dYLogService = dYLogService;
        }

        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            RelationRFIDQRCodeDataObject result = new RelationRFIDQRCodeDataObject();
            try
            {
                result = this.relationRFIDQRCodeService.GetByID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：relationRFIDQRCode/GetByID,错误信息：" + e.ToString() });
            }
            return new DyResult(result);
        }
        [HttpPost]
        public RelationRFIDQRCodeDataObject Update([FromBody] RelationRFIDQRCodeDataObject relationRFIDQRCode)
        {

            RelationRFIDQRCodeDataObject result = new RelationRFIDQRCodeDataObject();
            try
            {
                result = this.relationRFIDQRCodeService.Update(relationRFIDQRCode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：relationRFIDQRCode/Update,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public RelationRFIDQRCodeDataObject Add([FromBody]RelationRFIDQRCodeDataObject relationRFIDQRCode)
        {
            
            RelationRFIDQRCodeDataObject result = new RelationRFIDQRCodeDataObject();
            try
            {
                result = this.relationRFIDQRCodeService.Add(relationRFIDQRCode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：relationRFIDQRCode/Add,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public RelationRFIDQRCodeDataObject AddQRCode([FromBody]RelationRFIDQRCodeDataObject relationRFIDQRCode)
        {

            RelationRFIDQRCodeDataObject result = new RelationRFIDQRCodeDataObject();
            try
            {
                result = this.relationRFIDQRCodeService.AddQRCode(relationRFIDQRCode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：relationRFIDQRCode/AddQRCode,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public RelationRFIDQRCodeDataObject AddRFID([FromBody]RelationRFIDQRCodeDataObject relationRFIDQRCode)
        {

            RelationRFIDQRCodeDataObject result = new RelationRFIDQRCodeDataObject();
            try
            {
                result = this.relationRFIDQRCodeService.AddRFID(relationRFIDQRCode);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：relationRFIDQRCode/AddRFID,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpPost]
        public IList<RelationRFIDQRCodeDataObject> GetQRCodeList()
        {
            IList<RelationRFIDQRCodeDataObject> result = new List<RelationRFIDQRCodeDataObject>();
            try
            {
                result = this.relationRFIDQRCodeService.GetQRCodeList();
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：relationRFIDQRCode/GetQRCodeList,错误信息：" + e.ToString() });
            }
            return result;
        }
        [HttpGet]
        public int Remove(int id)
        {
            int result = 0;
            try
            {
                result = this.relationRFIDQRCodeService.RemoveByID(id);
            }
            catch (Exception e)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "方法：relationRFIDQRCode/RemoveByID,错误信息：" + e.ToString() });
            }
            return result;
        }
    }
}