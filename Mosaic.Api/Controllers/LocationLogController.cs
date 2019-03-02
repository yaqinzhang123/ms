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
    [Route("api/LocationLog/[action]")]
    public class LocationLogController : Controller
    {
        private ILocationLogService locationLogService;
        private IOperationService operationService;

        public LocationLogController(ILocationLogService locationLogService,IOperationService operationService)
        {
            this.locationLogService = locationLogService;
            this.operationService = operationService;
        }
        [HttpGet]
        public IList<LocationLogDataObject> GetLocationLogList()
        {

            return this.locationLogService.GetList();
        }
       
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.locationLogService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public LocationLogDataObject Update([FromBody]LocationLogDataObject locationLog)
        {
            return this.locationLogService.Update(locationLog);
        }
        [HttpPost]
        public string AddList([FromBody]IList<LocationLogTransfer> locationLogTransferList)
        {
            //int n= this.locationLogService.AddList(locationLogTransferList);
            //return "1";
            string result = null;
            for (int i = 0; i < locationLogTransferList.Count(); i++)
            {
                result = this.Add(locationLogTransferList[i]);
            }
            return "1";
        }
        [HttpPost]
        public string Add([FromBody]LocationLogTransfer locationLogTransfer)
        {
            //OperationDataObject operation = this.operationService.GetOperationByProductionLine(locationLogTransfer.ProductionLineID);
            //if (!operation.State)
            //    return "1";
            try
            {
                LocationLogDataObject locationLog = new LocationLogDataObject();
                locationLog.Name = locationLogTransfer.Name;


                locationLog.ProductionLineID = locationLogTransfer.ProductionLineID;
                locationLog.Time = DateTime.MinValue.AddTicks(locationLogTransfer.Time);
                LocationLogDataObject loc= this.locationLogService.Add(locationLog);
                if (loc == null)
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
            return this.locationLogService.RemoveByID(id);
        }
    }
}