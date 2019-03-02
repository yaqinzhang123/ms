using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Api.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    public class HomeController : Controller
    {
        private ICompanyService companyService;

        public HomeController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }
        [HttpGet("/")]
        [HttpPost("/")]
        public IActionResult Index()
        {
            ViewBag.Company = this.companyService.GetByID(2);
            return View();
        }
        public DyResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            return new DyResult(error.Message, DyStatusCode.Fail);
        }
    }
}
