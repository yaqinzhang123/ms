using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenticationServer.Models;
using AuthenticationServer.ServiceContracts;
using AuthenticationServer.DataObjects;
using AuthenticationServer.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationServer.Controllers
{
    [Produces("application/json")]
    public class ManagerController : Controller
    {
        public IAppInfoService AppInfoService { get; }
        public IManagerService ManagerService { get; }

        public ManagerController(IAppInfoService appInfoService,
            IManagerService managerService)
        {
            AppInfoService = appInfoService;
            ManagerService = managerService;
        }
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("Manager"))
                return RedirectToAction("Login");
            return RedirectToAction("Dashboard");
        }
        [HttpGet("[controller]/Login")]
        public IActionResult Login()
        {
            return  View();
        }
        [HttpPost("[controller]/Login")]
        public ApiResult CheckLogin([FromForm] string username,[FromForm]string password)
        {
            ManagerDataObject manager = this.ManagerService.CheckManager(username, password);
            if (manager != null)
            {
                HttpContext.Session.SetObjectAsJson("Manager", manager);
                return new ApiResult { Code = "Success", Message = "Sign in Authentication Server Success.", Data = "Dashboard" };
            }
            else
                return new ApiResult { Code = "Fail", Message = "Username or Password is incorect!" };
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("[controller]/CreateAccount")]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost("[controller]/CreateAccount")]
        public IActionResult CreateAccount(ManagerDataObject manager)
        {
            manager = this.ManagerService.Add(manager);
            HttpContext.Session.SetObjectAsJson("Manager", manager);
            return RedirectToAction("Dashboard");
        }
        public IActionResult Dashboard()
        {
            ManagerDataObject manager = HttpContext.Session.GetObjectFromJson<ManagerDataObject>("Manager");
            
            return View();
        }
    }
}
