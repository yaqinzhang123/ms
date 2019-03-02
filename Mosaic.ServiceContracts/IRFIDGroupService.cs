using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IRFIDGroupService : IService<RFIDGroupDataObject>
    {
        IList<RFIDGroupDataObject> GetListByInvoice(int id);
        IList<RFIDGroupDataObject> GetListByCompanyID(int id);
        RFIDGroupDataObject UpdateRFID(RFIDGroupDataObject dataObject);
        RFIDGroupDataObject UpdateQRCode(RFIDGroupDataObject dataObject);
        RFIDGroupDataObject GetRFIDGroup(int id);
        RFIDGroupDataObject Get(int id);
    }
}
