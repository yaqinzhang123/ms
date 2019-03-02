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
    [Route("api/UserInfo/[action]")]
    public class UserInfoController : Controller
    {
        private IUserInfoService userInfoService;
        private readonly IDYLogService dYLogService;

        public UserInfoController(IUserInfoService userInfoService,IDYLogService dYLogService)
        {
            this.userInfoService = userInfoService;
            this.dYLogService = dYLogService;
        }
        [HttpGet]
        public IList<UserInfoDataObject> GetUserInfoList()
        {

            return this.userInfoService.GetList();
        }
        [HttpGet]
        public IList<UserInfoDataObject> GetListByCompanyID(int id)
        {
            return this.userInfoService.GetListByCompanyID(id);
        }
        [HttpGet("{id}")]
        public DyResult Get(int id)
        {
            var result = this.userInfoService.Get(id);
            return new DyResult(result);
        }
        [HttpPost]
        public UserInfoDataObject Update([FromBody]UserInfoDataObject userInfo)
        {
            if (userInfo.Name == "putongyonghu")
                return new UserInfoDataObject();
            this.dYLogService.Add(new DYLogDataObject() { Memo = "修改用户：" + userInfo.Name + ",用户id："+userInfo.ID });
            return this.userInfoService.Update(userInfo);
        }
        [HttpPost]
        public UserInfoMessage AddUser([FromBody]UserInfoDataObject user)
        {
            if (user == null)
                return new UserInfoMessage() { };
            if (this.userInfoService.Exists(user.Name))
                return new UserInfoMessage() { Code = 0, Message = string.Format("用户名:{0},已存在！", user.Name) };
            user = this.userInfoService.Add(user);
            if (user != null)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "新增用户：用户名：" + user.Name + ",公司id:"+user.CompanyID+"，操作人id："+user.ID });
                return new UserInfoMessage() { Code = 2, Message = "添加用户成功!", Data = user };
            }
            return new UserInfoMessage() { Code = 0, Message = "添加用户失败!" };
        }
        [HttpPost]
        public UserInfoMessage Login([FromBody]UserInfoDataObject user)
        {
            if(String.IsNullOrEmpty(user.Name)||String.IsNullOrEmpty(user.Password))
                return new UserInfoMessage() { Code = 0, Message = "请输入正确的用户名和密码!" };
            if (!this.userInfoService.Exists(user.Name))
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "用户登录：用户名：" + user.Name + ",用户名不存在!" });
                return new UserInfoMessage() { Code = 0, Message = "用户名不存在!" };
            }
            UserInfoDataObject userInfo = this.userInfoService.CheckUser(user.Name, user.Password);
            if (userInfo == null)
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "用户登录：用户名：" + user.Name + ",用户名密码不匹配!" });
                return new UserInfoMessage() { Code = 1, Message = "用户名密码不匹配!" };
            }
            else
                this.dYLogService.Add(new DYLogDataObject() { Memo = "用户登录：用户名：" + userInfo.Name });
                return new UserInfoMessage() { Code = 2, Data = userInfo };
        }
        [HttpPost]
        public DyMessage RemoveUser([FromBody]UserInfoDataObject user)
        {
            if(user.ID==0)
                return new DyMessage() { Code = 0, Message = "" };
            if (this.userInfoService.RemoveByID(user.ID)>0)
            {
                return new DyMessage() { Code = 2, Message = "删除用户成功!" };
            }
            else
                return new DyMessage() { Code = 0, Message = "删除用户失败!" };
        }
        [HttpPost]
        public DyMessage UpdateUser([FromBody]UserInfoDataObject user)
        {
            if(user.ID==0||user==null)
                return new DyMessage() { Code = 0, Message = "" };
            if (this.userInfoService.UpdateUser(user))
            {
                this.dYLogService.Add(new DYLogDataObject() { Memo = "修改用户UpdateUser：" + user.Name + ",用户id：" + user.ID });
                return new DyMessage() { Code = 2, Message = "修改成功!" };
            }
            else
            {
                return new DyMessage() { Code = 0, Message = "修改失败!" };
            }
        }
        [HttpGet]
        public int Remove(int id)
        {
            this.dYLogService.Add(new DYLogDataObject() { Memo = "删除用户Remove：" + id });
            return this.userInfoService.RemoveByID(id);
        }
     
        [HttpPost]
        public bool ChangePassword([FromBody]UserInfoDataObject user)
        {
            if (user == null)
                return false;
            return this.userInfoService.ChangePassword(user);
        }
        [HttpGet]
        public IList<UserInfoDataObject> UserInfoQuery(string name, int id)
        {
            return this.userInfoService.UserInfoQuery(name, id);
        }
    }
}