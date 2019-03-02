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
    [Route("api/DYLog/[action]")]
    public class DYLogController : Controller
    {
        private IDYLogService dYLogService;

        public DYLogController(IDYLogService dYLogService)
        {
            this.dYLogService = dYLogService;
        }
        [HttpGet]
        public IList<DYLogDataObject> GetDYLogList()
        {

            return this.dYLogService.GetList();
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.dYLogService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public DYLogDataObject Update([FromBody] DYLogDataObject dYLog)
        {
            return this.dYLogService.Update(dYLog);
        }
        [HttpPost]
        public DTOMessage<DYLogDataObject> Add([FromBody]DYLogDataObject dYLog)
        {
            DYLogDataObject dYLogDataObject=dYLogService.Add(dYLog);
            return  new Msg.DTOMessage<DYLogDataObject>() { Code = 2 ,Data = dYLogDataObject };
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.dYLogService.RemoveByID(id);
        }
    }
}