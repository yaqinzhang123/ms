using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IRFIDRecordService : IService<RFIDRecordDataObject>
    {
      
        List<int> GetProductionLineIDList();
        IList<RFIDRecordDataObject> GetUnhandleListByProduction(int lineId);
        int AddList(IList<RFIDRecordDataObject> rfidList);
    }
}
