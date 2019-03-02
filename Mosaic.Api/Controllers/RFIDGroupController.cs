using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/RFIDGroup/[action]")]
    public class RFIDGroupController : Controller
    {
        private IRFIDGroupService rFIDGroupService;

        public RFIDGroupController(IRFIDGroupService rFIDGroupService)
        {
            this.rFIDGroupService = rFIDGroupService;
        }
        [HttpGet]
        public IList<RFIDGroupDataObject> GetRFIDGroupList()
        {

            return this.rFIDGroupService.GetList();
        }
        //按照发货单查
        [HttpGet]
        public IList<RFIDGroupDataObject> GetListByInvoice(int id)
        {

            return this.rFIDGroupService.GetListByInvoice(id);
        }
        //按照公司查所有未关联的交货单
        [HttpGet]
        public IList<RFIDGroupDataObject> GetListByCompanyID(int id)
        {

            return this.rFIDGroupService.GetListByCompanyID(id);
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.rFIDGroupService.Get(id);
            return new DyResult(result);
        }
        [HttpGet]
        public RFIDGroupDataObject GetRFIDGroupByID(int id)
        {
            return this.rFIDGroupService.GetByID(id);
        }
        [HttpPost]
        public RFIDGroupDataObject Update([FromBody]RFIDGroupDataObject rFIDGroup)
        {
            return this.rFIDGroupService.Update(rFIDGroup);
        }
        [HttpPost]
        public RFIDGroupDataObject Add([FromBody]RFIDGroupDataObject rFIDGroup)
        {
            return this.rFIDGroupService.Add(rFIDGroup);
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.rFIDGroupService.RemoveByID(id);
        }
        [HttpPost]
        public RFIDGroupDataObject UpdateRFID([FromBody]RFIDGroupDataObject dataObject)
        {
            return this.rFIDGroupService.UpdateRFID(dataObject);
        }
        [HttpPost]
        public RFIDGroupDataObject UpdateQRCode([FromBody]RFIDGroupDataObject dataObject)
        {
            return this.rFIDGroupService.UpdateQRCode(dataObject);
        }
    }
}