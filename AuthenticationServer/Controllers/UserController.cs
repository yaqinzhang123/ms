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
    [Route("api/User/[action]")]
    public class UserController : Controller
    {
        public UserController(IUserInfoService userInfoService)
        {
            UserInfoService = userInfoService;
        }

        public IUserInfoService UserInfoService { get; }


        [HttpGet]
        public IList<UserInfoDataObject> Get()
        {
            return this.UserInfoService.GetList();
        }

        [HttpGet("{id}")]
        public UserInfoDataObject Get(int id)
        {
            return this.UserInfoService.GetByID(id);
        }
        
        [HttpPost]
        public ApiResult Add([FromBody] UserInfoDataObject userInfo)
        {
            try
            {
                var result = this.UserInfoService.Add(userInfo);
                if (result != null)
                    return new ApiResult() { Code = "0", Message = "success", Data = result };
                else
                    return new ApiResult() { Code = "1", Message = "faild" };
            }
            catch (Exception ex)
            {
                return new ApiResult() { Code = "2", Message = ex.Message };
                
            }
        }

        [HttpDelete("{id}")]
        public ApiResult Delete(int id)
        {
            try
            {
                var result = this.UserInfoService.RemoveByID(id);
                if (result > 0)
                    return new ApiResult() { Code = "0", Message = "success" };
                else
                    return new ApiResult() { Code = "1", Message = "faild" };
            }
            catch (Exception ex)
            {
                return new ApiResult() { Code = "2", Message = ex.Message };

            }
        }


        [HttpPost]
        public ApiResult Update([FromBody] UserInfoDataObject userInfo)
        {
            try
            {
                var result = this.UserInfoService.Update(userInfo);
                if (result != null)
                    return new ApiResult() { Code = "0", Message = "success", Data = result };
                else
                    return new ApiResult() { Code = "1", Message = "faild" };
            }
            catch (Exception ex)
            {
                return new ApiResult() { Code = "2", Message = ex.Message };

            }
        }
    }
}