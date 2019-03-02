using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface ICarInfoService : IService<CarInfoDataObject>
    {
       // IList<CarInfoDataObject> GetListByLineId(int lineId, int v);
    }
}
