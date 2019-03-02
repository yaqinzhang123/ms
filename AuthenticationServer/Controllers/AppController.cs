using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServer.DataObjects;
using AuthenticationServer.ResultModel;
using AuthenticationServer.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.Controllers
{
    [Produces("application/json")]
    [Route("api/App/[action]")]
    public class AppController : Controller
    {
        public AppController(IAppInfoService appInfoService)
        {
            AppInfoService = appInfoService;
        }

        public IAppInfoService AppInfoService { get; }

        [HttpGet]
        public IList<AppInfoDataObject> Get()
        {
            return this.AppInfoService.GetList();
        }
        [HttpGet("{id}")]
        public AppInfoDataObject Get(int id)
        {
            return this.AppInfoService.GetByID(id);
        }

        [HttpPost]
        public ApiResult Add([FromBody] AppInfoDataObject appinfo)
        {
            ApiResult result = new ApiResult();
            result.Code = "0";
            result.Message = "success";
            result.Data = this.AppInfoService.Add(appinfo);
            return result;
        }
        [HttpDelete("{id}")]
        public ApiResult Delete(int id)
        {
            try
            {
                if (this.AppInfoService.RemoveByID(id) > 0)
                    return new ApiResult { Code = "0", Message = "success" };
                else
                    return new ApiResult { Code = "1", Message = "faild" };
            }
            catch (Exception ex)
            {
                return new ApiResult {Code = "2",Message = ex.Message};
            }
        }
       
        [HttpPost]
        public ApiResult Update([FromBody] AppInfoDataObject appinfo)
        {
            try
            {
                var result = this.AppInfoService.Update(appinfo);
                if (result != null)
                    return new ApiResult { Code = "0", Message = "success", Data = result };
                else
                    return new ApiResult { Code = "1", Message = "faild" };
            }
            catch(Exception ex)
            {
                return new ApiResult { Code = "2", Message = ex.Message };
            }
        }
    }
}