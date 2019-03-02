
using AuthenticationServer.DataObjects;
using DYFramework.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.ServiceContracts
{
    public interface IUserInfoService : IService<UserInfoDataObject>
    {
        UserInfoDataObject Get(string userName, string password);
    }
}
