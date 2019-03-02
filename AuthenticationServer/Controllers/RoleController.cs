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
    [Route("api/Role/[action]")]
    public class RoleController : Controller
    {
        public IRoleService RoleService { get; }
        public RoleController(IRoleService roleService)
        {
            RoleService = roleService;
        }

        [HttpGet]
        public IList<RoleDataObject> Get()
        {
            return this.RoleService.GetList();
        }

        [HttpGet("{id}")]
        public RoleDataObject Get(int id)
        {
            return this.RoleService.GetByID(id);
        }

        [HttpPost]
        public ApiResult Add([FromBody] RoleDataObject role)
        {
            try
            {
                var result = this.RoleService.Add(role);
                if (result != null)
                    return new ApiResult { Code = "0", Message = "success", Data = result };
                else
                    return new ApiResult { Code = "1", Message = "faild" };
            }
            catch (Exception ex)
            {
                return new ApiResult { Code = "2", Message = ex.Message };
            }
        }

        [HttpDelete("{id}")]
        public ApiResult Delete(int id)
        {
            try
            {
                if (this.RoleService.RemoveByID(id) > 0)
                    return new ApiResult { Code = "0", Message = "success" };
                else
                    return new ApiResult { Code = "1", Message = "faild" };
            }
            catch (Exception ex)
            {
                return new ApiResult { Code = "2", Message = ex.Message };
            }
        }

        [HttpPost]
        public ApiResult Update([FromBody] RoleDataObject role)
        {
            try
            {
                var result = this.RoleService.Update(role);
                if (result != null)
                    return new ApiResult { Code = "0", Message = "success", Data = result };
                else
                    return new ApiResult { Code = "1", Message = "faild" };
            }
            catch (Exception ex)
            {

                return new ApiResult { Code = "2", Message = ex.Message };
            }
        }
    }
}