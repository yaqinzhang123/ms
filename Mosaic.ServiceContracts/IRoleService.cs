using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IRoleService : IService<RoleDataObject>
    {
        IList<RoleDataObject> GetListByCompanyID(int id);
        IList<RoleDataObject> RoleQuery(string name, int id);
        bool Exists(string name);
        RoleDataObject Get(int id);
    }
}
