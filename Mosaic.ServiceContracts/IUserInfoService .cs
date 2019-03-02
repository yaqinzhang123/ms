using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IUserInfoService : IService<UserInfoDataObject>
    {
        bool Exists(string name);
        bool ChangePassword(UserInfoDataObject user);
        UserInfoDataObject CheckUser(string userName, string password);
        bool UpdateUser(UserInfoDataObject user);
        IList<UserInfoDataObject> GetListByCompanyID(int id);
        IList<UserInfoDataObject> UserInfoQuery(string name, int id);
        UserInfoDataObject Get(int id);
        //UserInfoDataObject AddRole(UserInfoDataObject user);
    }
}
