using AutoMapper;
using DYFramework.Application;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.Infrastructure;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class UserInfoService: Service<UserInfoDataObject, UserInfo>, IUserInfoService
    {
        public UserInfoService(IUserInfoRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public UserInfoDataObject CheckUser(string name, string password)
        {
            UserInfoDataObject userInfo = Mapper.Map<UserInfo, UserInfoDataObject>(this.repository.Get(p => p.Name == name && p.Password == password).FirstOrDefault());
            if (userInfo == null || userInfo.ID == 0)
                return userInfo;
            List<RightsDataObject> newRightsList = new List<RightsDataObject>();
            IList<UserRoleDataObject> userRoleList = this.GetRoles(userInfo.ID);
            userInfo.UserRole = userRoleList;
            for (int j = 0; j < userRoleList.Count(); j++)
            {
                newRightsList.AddRange(userRoleList[j].Role.RightsList);
            }
            userInfo.Rights = newRightsList;
            return userInfo;
        }
        public UserInfoDataObject Get(int id)
        {
            UserInfoDataObject userInfo = Mapper.Map<UserInfo, UserInfoDataObject>(this.repository.Get(p => p.ID == id).FirstOrDefault());
            userInfo.UserRole = this.GetRoles(id);
            return userInfo;
        }
        public bool ChangePassword(UserInfoDataObject user)
        {
            UserInfo userInfo = this.repository.Get(p=>p.ID==user.ID).FirstOrDefault();
            userInfo.Password = user.Password;
            this.repository.Update(userInfo);
            return this.repository.Commit() > 0;
        }
        public bool Exists(string name)
        {
            return this.repository.Exists(p =>!p.Deleted&& p.Name == name);
        }
        public bool UpdateUser(UserInfoDataObject user)
        {
            if (this.repository.Exists(p =>!p.Deleted && p.ID != user.ID && p.Name == user.Name))
                return false;
            UserInfo userInfo = this.repository.Get(p=>p.ID==user.ID).FirstOrDefault();
            user.Password = userInfo.Password;
            userInfo = Mapper.Map(user, userInfo);
            this.repository.Update(userInfo);
            return this.repository.Commit() > 0;
        }
        //分公司查看
        public IList<UserInfoDataObject> GetListByCompanyID(int id)
        {
            IList<UserInfoDataObject> userInfoList = Mapper.Map<IList<UserInfo>, IList<UserInfoDataObject>>(this.repository.Get(p => p.CompanyID == id&!p.Deleted&p.Name!="super").ToList());
            IList<UserInfoDataObject> newUserList = new List<UserInfoDataObject>();
           
            for (int i = 0; i < userInfoList.Count(); i++)
            {
                IList<UserRoleDataObject> userRoleList = this.GetRoles(userInfoList[i].ID);
                userInfoList[i].UserRole = userRoleList;
                newUserList.Add(userInfoList[i]);
            }
            return newUserList;
        }
        //拿角色
        public IList<UserRoleDataObject> GetRoles(int userInfoID)
        {
            IList<UserRole> userRoles = this.repository.Context.Get<UserRole>(p => p.UserInfoID == userInfoID).ToList();
            IList<UserRole> newRoles = new List<UserRole>();
            for (int i = 0; i < userRoles.Count(); i++)
            {
                Role role = this.repository.Context.Get<Role>(p => p.ID == userRoles[i].RoleID).FirstOrDefault();
                if (role != null)
                {
                    role.rightsList = this.GetRights(userRoles[i].RoleID).ToList();
                    userRoles[i].Role = role;
                    newRoles.Add(userRoles[i]);
                }
            }
            return Mapper.Map<IList<UserRole>, IList<UserRoleDataObject>>(newRoles);
        }
        //拿权限
        public IList<Rights> GetRights(int roleID)
        {
            IList<Rights> rightsList = this.repository.Context.Get<Rights>(p => p.RoleID == roleID).ToList();
            IList<Rights> newRights = new List<Rights>();
            for (int i = 0; i < rightsList.Count(); i++)
            {
                Rights rights = this.repository.Context.Get<Rights>(p => p.ID == rightsList[i].ID).FirstOrDefault();
                newRights.Add(rights);
            }
            return newRights;
        }
        //public UserInfoDataObject AddRole(UserInfoDataObject user)
        //{
        //    Role role = this.repository.Context.Get<Role>(p => p.Name==user.RoleName).FirstOrDefault();
        //    UserInfo userInfo = this.repository.Get(p => p.ID == user.ID).FirstOrDefault();
        //    this.repository.Update(userInfo);
        //    this.repository.Commit();
        //    return Mapper.Map<UserInfo, UserInfoDataObject>(userInfo);
        //}

        public IList<UserInfoDataObject> UserInfoQuery(string name, int id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return this.GetListByCompanyID(id);
            }
            var query = this.repository.Get(p => p.CompanyID == id).ToList();
            query = query.Where(p => p.Name.Contains(name.Trim())).ToList();
            return Mapper.Map<IList<UserInfo>, IList<UserInfoDataObject>>(query);
        }
    }
}
