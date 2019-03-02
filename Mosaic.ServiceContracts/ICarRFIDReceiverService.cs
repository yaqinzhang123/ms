using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface ICarRFIDReceiverService : IService<CarRFIDReceiverDataObject>
    {
        IList<RFIDUpdateDataObject> GetListByTime(GroupDataObject group);
    }
}
