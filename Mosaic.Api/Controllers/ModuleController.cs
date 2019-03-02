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
    [Route("api/Module/[action]")]
    public class ModuleController : Controller
    {
        private IModuleService moduleService;

        public ModuleController(IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }
        [HttpGet]
        public IList<ModuleDataObject> GetModuleList()
        {

            return this.moduleService.GetList();
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.moduleService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public ModuleDataObject Update([FromBody]ModuleDataObject module)
        {
            return this.moduleService.Update(module);
        }
        [HttpPost]
        public ModuleDataObject Add([FromBody]ModuleDataObject module)
        {
            return this.moduleService.Add(module);
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.moduleService.RemoveByID(id);
        }
    }
}