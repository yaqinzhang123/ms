using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Api.Msg;
using Mosaic.Application.Impl;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ControlInfo/[action]")]
    public class ControlInfoController : Controller
    {
        private IControlInfoService controlInfoService;
        private IOperationService operationService;
        public ControlInfoController(IControlInfoService controlInfoService, IOperationService operationService)
        {
            this.controlInfoService = controlInfoService;
            this.operationService = operationService;
        }
        [HttpGet]
        public IList<ControlInfoDataObject> GetControlInfoList()
        {

            return this.controlInfoService.GetList();
        }
       
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.controlInfoService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public ControlInfoDataObject Update( ControlInfoDataObject controlInfo)
        {
            return this.controlInfoService.Update(controlInfo);
        }
        [HttpPost]
        public string AddList([FromBody]IList<ControlInfoTransfer> controlInfoTransferList)
        {
            string result=null;
            for(int i = 0; i < controlInfoTransferList.Count(); i++)
            {
               result= this.Add(controlInfoTransferList[i]);
            }
            return result;

        }
        [HttpPost]
        public string Add(ControlInfoTransfer controlInfoTransfer)
        {
            //OperationDataObject operation = this.operationService.GetOperationByProductionLine(carInfoTransfer.ProductionLineID);
            //if (!operation.State)
            //    return "1";
            try
            {
                ControlInfoDataObject controlInfo = new ControlInfoDataObject();
                controlInfo.Time = DateTime.MinValue.AddTicks(controlInfoTransfer.Time);
                controlInfo.DI1 = controlInfoTransfer.DI1;
                controlInfo.DI2 = controlInfoTransfer.DI2;
                controlInfo.DI3 = controlInfoTransfer.DI3;
                controlInfo.DI4 = controlInfoTransfer.DI4;
                controlInfo.DI5 = controlInfoTransfer.DI5;
                controlInfo.DI6 = controlInfoTransfer.DI6;
                controlInfo.DI7 = controlInfoTransfer.DI7;
                controlInfo.DI8 = controlInfoTransfer.DI8;
                controlInfo.ProductionLineID = controlInfoTransfer.ProductionLineID;
                controlInfo.Flag = controlInfoTransfer.Flag;
                ControlInfoDataObject cont=this.controlInfoService.Add(controlInfo);
                if (cont == null)
                    return "0";
                return "1";
            }catch(Exception e)
            {
                return e.ToString();
            }
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.controlInfoService.RemoveByID(id);
        }
    }
}