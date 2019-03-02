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
    [Route("api/Agency/[action]")]
    public class AgencyController : Controller
    {
        private IAgencyService agencyService;

        public AgencyController(IAgencyService agencyService)
        {
            this.agencyService = agencyService;
        }
        [HttpGet]
        public IList<AgencyDataObject> GetAgencyList()
        {

            return this.agencyService.GetList();
        }
        [HttpGet]
        public IList<AgencyDataObject> GetListByCompanyID(int id,string grade)
        {
            return this.agencyService.GetListByCompanyID(id,grade);
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.agencyService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public AgencyDataObject Update([FromBody] AgencyDataObject agency)
        {
            return this.agencyService.Update(agency);
        }
        [HttpPost]
        public DTOMessage<AgencyDataObject> Add([FromBody]AgencyDataObject agency)
        {
            if (this.agencyService.Exists(agency.Name))
            {
                return new Msg.DTOMessage<AgencyDataObject>() { Code = 1, Message = "代理商名称存在!" };
            }
            AgencyDataObject agencyDataObject=agencyService.Add(agency);
            return  new Msg.DTOMessage<AgencyDataObject>() { Code = 2 ,Data = agencyDataObject };
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.agencyService.RemoveByID(id);
        }
    }
}