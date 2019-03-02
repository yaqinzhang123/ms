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
    [Route("api/Role/[action]")]
    public class RoleController : Controller
    {
        private IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        [HttpGet]
        public IList<RoleDataObject> GetRoleList()
        {

            return this.roleService.GetList();
        }
        [HttpGet]
        public IList<RoleDataObject> GetListByCompanyID(int id)
        {
            return this.roleService.GetListByCompanyID(id);
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.roleService.Get(id);
            return new DyResult(result);
        }
        [HttpPost]
        public RoleDataObject Update([FromBody] RoleDataObject role)
        {
            return this.roleService.Update(role);
        }
        [HttpPost]
        public DTOMessage<RoleDataObject> Add([FromBody]RoleDataObject role)
        {
            if (this.roleService.Exists(role.Name))
            {
                return new Msg.DTOMessage<RoleDataObject>() { Code = 1, Message = "角色名称已存在!" };
            }
            RoleDataObject roleDataObject = roleService.Add(role);
            return new Msg.DTOMessage<RoleDataObject>() { Code = 2, Data = roleDataObject };
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.roleService.RemoveByID(id);
        }
        [HttpGet]
        public IList<RoleDataObject> RoleQuery(string name, int id)
        {
            return this.roleService.RoleQuery(name,id);
        }
    }
}