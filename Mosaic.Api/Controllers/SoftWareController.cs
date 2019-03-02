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
    [Route("api/SoftWare/[action]")]
    public class SoftWareController : Controller
    {
        private ISoftWareService softWareService;

        public SoftWareController(ISoftWareService softWareService)
        {
            this.softWareService = softWareService;
        }
        [HttpGet]
        public IList<SoftWareDataObject> GetSoftWareList()
        {

            return this.softWareService.GetList();
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.softWareService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public SoftWareDataObject Update([FromBody]SoftWareDataObject softWare)
        {
            return this.softWareService.Update(softWare);
        }
        [HttpPost]
        public SoftWareDataObject Add([FromBody]SoftWareDataObject softWare)
        {
            return this.softWareService.Add(softWare);
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.softWareService.RemoveByID(id);
        }
    }
}