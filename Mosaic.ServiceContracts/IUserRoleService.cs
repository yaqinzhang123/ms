using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IUserRoleService : IService<UserRoleDataObject>
    {
       
        IList<UserRoleDataObject> AddRole(UserRoleDataObject user);
        IList<UserRoleDataObject> GetRoles(int userInfoID);
    }
}
