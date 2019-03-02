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
    [Route("api/Rights/[action]")]
    public class RightsController : Controller
    {
        private IRightsService rightsService;

        public RightsController(IRightsService rightsService)
        {
            this.rightsService = rightsService;
        }
        [HttpGet]
        public IList<RightsDataObject> GetRightsList()
        {

            return this.rightsService.GetList();
        }

        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.rightsService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public RightsDataObject Update([FromBody]RightsDataObject rights)
        {
            return this.rightsService.Update(rights);
        }
        [HttpPost]
        public RightsDataObject Add([FromBody]RightsDataObject rights)
        {
            if (String.IsNullOrWhiteSpace(rights.Factory) || String.IsNullOrWhiteSpace(rights.SoftName))
                return new RightsDataObject();
            return this.rightsService.Add(rights);
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.rightsService.RemoveByID(id);
        }
        [HttpPost]
        public CompanyDataObject RemoveRights([FromBody]RightsDataObject dataObject)
        {
            return this.rightsService.RemoveRights(dataObject);
        }
        [HttpGet]
        public RightsDataObject GetByRoleID(int id)
        {
            return this.rightsService.GetByRoleID(id);
        }
    }
}