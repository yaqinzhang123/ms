using AuthenticationServer.DataObjects;
using DYFramework.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.ServiceContracts
{
    public interface IManagerService : IService<ManagerDataObject>
    {
        ManagerDataObject CheckManager(string username, string password);
    }
}
