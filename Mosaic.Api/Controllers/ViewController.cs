using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    public class ViewController : Controller
    {
        private readonly IQRCodeService qRCodeService;

        public ViewController(IQRCodeService qRCodeService)
        {
            this.qRCodeService = qRCodeService;
        }
        public IActionResult Index(int? cid,string startTime,string endTime)
        {
            cid = cid == null ? 0 : cid.Value;
            //ViewBag.TotalQRCodeList = this.qRCodeService.GetTotal();
            return View();
        }
    }
}