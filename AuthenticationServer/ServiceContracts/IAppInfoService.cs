using AuthenticationServer.DataObjects;
using DYFramework.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.ServiceContracts
{
    public interface IAppInfoService:IService<AppInfoDataObject>
    {
    }
}
