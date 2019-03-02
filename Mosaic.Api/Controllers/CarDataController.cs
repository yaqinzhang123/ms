using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/CarData/[action]")]
    public class CarDataController : Controller
    {
        private readonly ICarDataService carDataService;

        public CarDataController(ICarDataService carDataService)
        {
            this.carDataService = carDataService;
        }
        [HttpPost]
        public string Add([FromBody] CarDataTransfer carDataTransfer)
        {
            this.carDataService.Add(carDataTransfer);
            return "1";
        }
        [HttpGet]
        public IList<CarDataDataObject> GetList()
        {
            return this.carDataService.GetList();
        }
    }
}