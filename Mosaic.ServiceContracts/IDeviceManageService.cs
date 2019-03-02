using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IDeviceManageService : IService<DeviceManageDataObject>
    {
        IList<DeviceManageDataObject> GetListByCompanyID(int id,string memo);
        bool Exists(string name);
    }
}
