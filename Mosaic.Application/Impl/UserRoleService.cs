using AutoMapper;
using DYFramework.Application;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Application.Impl
{
    public class UserRoleService : Service<UserRoleDataObject, UserRole>, IUserRoleService
    {
        public UserRoleService(IUserRoleRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public IList<UserRoleDataObject> GetRoles(int userInfoID)
        {
            IList<UserRole> userRoles = this.repository.Get(p => p.UserInfoID == userInfoID).ToList();
            IList<UserRole> newRoles = new List<UserRole>();
            for(int i = 0; i < newRoles.Count(); i++)
            {
                Role role = this.repository.Context.Get<Role>(p => p.ID == userRoles[i].RoleID).FirstOrDefault();
                userRoles[i].Role = role;
                newRoles.Add(userRoles[i]);
            }
            return Mapper.Map<IList<UserRole>, IList<UserRoleDataObject>>(newRoles);
        }
        public IList<UserRoleDataObject> AddRole(UserRoleDataObject user)
        {
            UserRole userRole = this.repository.Create();
            int roleid = this.repository.Context.Get<Role>(p => p.Name == user.RoleName).FirstOrDefault().ID;
            UserRole userrole = this.repository.Get(p => p.UserInfoID == user.UserInfoID && p.RoleID == roleid).FirstOrDefault();
            if(userrole == null)
            {
                userRole.RoleID = roleid;
                userRole.UserInfoID = user.UserInfoID;
                this.repository.Add(userRole);
                this.repository.Commit();
            }
            IList<UserRole> userRoles = this.repository.Get(p => p.UserInfoID == user.UserInfoID).ToList();
            return Mapper.Map<IList<UserRole>, IList<UserRoleDataObject>>(userRoles);
        }
    }
}
