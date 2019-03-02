using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/ProductionLine/[action]")]
    public class ProductionLineController : Controller
    {
        private IProductionLineService productionLineService;

        public ProductionLineController(IProductionLineService productionLineService)
        {
            this.productionLineService = productionLineService;
        }
        [HttpGet]
        public IList<ProductionLineDataObject> GetProductionLineList()
        {
            return this.productionLineService.GetList();
        }
        [HttpGet]
        public IList<ProductionLineDataObject> GetProductionLineByCompanyID(int id)
        {
            return this.productionLineService.GetProductionLinesByCompanyID(id);
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.productionLineService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public ProductionLineDataObject Update([FromBody]ProductionLineDataObject productionLine)
        {
            return this.productionLineService.Update(productionLine);
        }
        [HttpPost]
        public DTOMessage<ProductionLineDataObject> Add([FromBody]ProductionLineDataObject productionLine)
        {
            if (this.productionLineService.Exists(productionLine.Name))
            {
                return new Msg.DTOMessage<ProductionLineDataObject>() { Code = 1, Message = "产线名称已存在!" };
            }
            ProductionLineDataObject productionLineDataObject = productionLineService.Add(productionLine);
            return new Msg.DTOMessage<ProductionLineDataObject>() { Code = 2, Data = productionLineDataObject };
        }
        [HttpGet]
        public IList<ProductionLineDataObject> GetProductionLines(int CompanyID)
        {
            return this.productionLineService.GetProductionLinesByCompanyID(CompanyID);
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.productionLineService.RemoveByID(id);
        }
        [HttpPost]
        public ProductionLineDataObject UpdateOperation([FromBody]ProductionLineDataObject productionLine)
        {
            return this.productionLineService.UpdateOperation(productionLine);
        }
    }
}