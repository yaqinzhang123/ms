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
    [Route("api/CarInfo/[action]")]
    public class CarInfoController : Controller
    {
        private ICarInfoService carInfoService;
        private IOperationService operationService;

        public CarInfoController(ICarInfoService carInfoService, IOperationService operationService)
        {
            this.carInfoService = carInfoService;
            this.operationService = operationService;
        }
        [HttpGet]
        public IList<CarInfoDataObject> GetCarInfoList()
        {

            return this.carInfoService.GetList();
        }
       
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.carInfoService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public CarInfoDataObject Update([FromBody]CarInfoDataObject carInfo)
        {
            return this.carInfoService.Update(carInfo);
        }
        [HttpPost]
        public string AddList([FromBody]IList<CarInfoTransfer> carInfoTransferList)
        {
            string result = null;
            for (int i = 0; i < carInfoTransferList.Count(); i++)
            {
                result = this.Add(carInfoTransferList[i]);
            }
            return result;

        }
        [HttpPost]
        public string Add([FromBody]CarInfoTransfer carInfoTransfer)
        {
            //OperationDataObject operation = this.operationService.GetOperationByProductionLine(carInfoTransfer.ProductionLineID);
            //if (!operation.State)
            //    return "1";
            try
            {
                CarInfoDataObject carInfo = new CarInfoDataObject();
                carInfo.TriggerTime = DateTime.MinValue.AddTicks(carInfoTransfer.TriggerTime);
                // carInfo.Leave= DateTime.MinValue.AddTicks(carInfoTransfer.LeaveTime);
                carInfo.Location = carInfoTransfer.Location;
                carInfo.ProductionLineID = carInfoTransfer.ProductionLineID;
                carInfo.Flag = carInfoTransfer.Flag;
                carInfo.Status = carInfoTransfer.Status;
                CarInfoDataObject car = this.carInfoService.Add(carInfo);
                if (car == null)
                {
                    return "0";

                }
                return "1";
            }catch (Exception e)
            {
                return e.ToString();
            }
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.carInfoService.RemoveByID(id);
        }
    }
}