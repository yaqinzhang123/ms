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
    [Route("api/CarRFIDReceiver/[action]")]
    public class CarRFIDReceiverController : Controller
    {
        private ICarRFIDReceiverService carRFIDReceiverService;
        private IOperationService operationService;

        public CarRFIDReceiverController(ICarRFIDReceiverService carRFIDReceiverService, IOperationService operationService)
        {
            this.carRFIDReceiverService = carRFIDReceiverService;
            this.operationService = operationService;
        }
        [HttpGet]
        public IList<CarRFIDReceiverDataObject> GetCarRFIDReceiverList()
        {

            return this.carRFIDReceiverService.GetList();
        }
       
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.carRFIDReceiverService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public CarRFIDReceiverDataObject Update([FromBody]CarRFIDReceiverDataObject carRFIDReceiver)
        {
            return this.carRFIDReceiverService.Update(carRFIDReceiver);
        }
        [HttpPost]
        public CarRFIDReceiverDataObject Add([FromBody]CarRFIDReceiverDataObject carRFIDReceiver)
        {
            return this.carRFIDReceiverService.Add(carRFIDReceiver);
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.carRFIDReceiverService.RemoveByID(id);
        }
        [HttpPost]
        public IList<RFIDUpdateDataObject> GetListByTime([FromBody]GroupDataObject group)
        {
            return this.carRFIDReceiverService.GetListByTime(group);
        }
    }
}