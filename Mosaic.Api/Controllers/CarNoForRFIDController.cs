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
    [Route("api/CarNoForRFID/[action]")]
    public class CarNoForRFIDController : Controller
    {
        private ICarNoForRFIDService carNoForRFIDService;
        private IOperationService operationService;

        public CarNoForRFIDController(ICarNoForRFIDService carNoForRFIDService, IOperationService operationService)
        {
            this.carNoForRFIDService = carNoForRFIDService;
            this.operationService = operationService;
        }
        [HttpGet]
        public IList<CarNoForRFIDDataObject> GetCarNoForRFIDList()
        {

            return this.carNoForRFIDService.GetList();
        }
       
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.carNoForRFIDService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public CarNoForRFIDDataObject Update([FromBody]CarNoForRFIDDataObject carNoForRFID)
        {
            return this.carNoForRFIDService.Update(carNoForRFID);
        }
        [HttpPost]
        public CarNoForRFIDDataObject Add([FromBody]CarNoForRFIDDataObject carNoForRFID)
        {
            return this.carNoForRFIDService.Add(carNoForRFID);
        }

        [HttpGet]
        public int Remove(int id)
        {
            return this.carNoForRFIDService.RemoveByID(id);
        }
    }
}