using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IRightsService : IService<RightsDataObject>
    { 
        RightsDataObject GetByRoleID(int id);
        CompanyDataObject RemoveRights(RightsDataObject dataObject);
    }
}
