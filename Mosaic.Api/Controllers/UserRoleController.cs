using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DYFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Application.Impl;
using Mosaic.Domain.Models;
using Mosaic.DTO;
using Mosaic.ServiceContracts;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/UserRole/[action]")]
    public class UserRoleController : Controller
    {
        private IUserRoleService userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            this.userRoleService = userRoleService;
        }
        [HttpGet]
        public IList<UserRoleDataObject> GetUserRoleList()
        {

            return this.userRoleService.GetList();
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.userRoleService.GetByID(id);
            return new DyResult(result);
        }
        [HttpGet]
        public IList<UserRoleDataObject> GetRoles(int userInfoID)
        {
            return this.userRoleService.GetRoles(userInfoID);
        }
        [HttpPost]
        public UserRoleDataObject Update([FromBody] UserRoleDataObject userRole)
        {
            return this.userRoleService.Update(userRole);
        }
        [HttpPost]
        public UserRoleDataObject Add([FromBody]UserRoleDataObject userRole)
        {
            return this.userRoleService.Add(userRole);
        }
        [HttpPost]
        public IList<UserRoleDataObject> AddRole([FromBody]UserRoleDataObject user)
        {
            return this.userRoleService.AddRole(user);
        }
        [HttpGet]
        public int Remove(int id)
        {
            return this.userRoleService.RemoveByID(id);
        }
    }
}