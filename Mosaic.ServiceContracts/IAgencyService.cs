using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IAgencyService : IService<AgencyDataObject>
    {
        IList<AgencyDataObject> GetListByCompanyID(int id, string grade);
        bool Exists(string name);
    }
}
