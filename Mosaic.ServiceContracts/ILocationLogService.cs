using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface ILocationLogService : IService<LocationLogDataObject>
    {
        LocationLogDataObject LastLocation(int lineID, DateTime leave);
        int AddList(IList<LocationLogTransfer> list);
    }
}
