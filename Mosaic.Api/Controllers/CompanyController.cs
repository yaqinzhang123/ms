using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Api.Msg;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Company/[action]")]
    public class CompanyController : Controller
    {
        private ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }
        [HttpGet]
        public IList<CompanyDataObject> GetCompanyList()
        {
            return this.companyService.GetList();
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.companyService.GetByID(id);
            return new DyResult(result);
        }
        [HttpPost]
        public CompanyDataObject Update([FromBody]CompanyDataObject company)
        {
            return this.companyService.Update(company);
        }
        [HttpPost]
        public DTOMessage<CompanyDataObject> Add([FromBody]CompanyDataObject company)
        {
            if (this.companyService.Exists(company.Name))
            {
                return new Msg.DTOMessage<CompanyDataObject>() { Code = 1, Message = "公司名称已存在!" };
            }
            CompanyDataObject companyDataObject = companyService.Add(company);
            return new Msg.DTOMessage<CompanyDataObject>() { Code = 2, Data = companyDataObject };
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.companyService.RemoveByID(id);
        }
        [HttpGet]
        public CompanyDataObject GetCompanyByQRCode(string qrCode)
        {
            return this.companyService.GetCompanyByQRCode(qrCode);
        }
        [HttpGet]
        public IList<string> GetShortCode()
        {
            return this.companyService.GetShortCode();
        }
    }
}