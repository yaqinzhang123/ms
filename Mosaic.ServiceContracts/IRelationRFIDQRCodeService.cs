using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IRelationRFIDQRCodeService : IService<RelationRFIDQRCodeDataObject>
    {
        RelationRFIDQRCodeDataObject AddQRCode(RelationRFIDQRCodeDataObject relationRFIDQRCode);
        RelationRFIDQRCodeDataObject AddRFID(RelationRFIDQRCodeDataObject relationRFIDQRCode);
        IList<RelationRFIDQRCodeDataObject> GetQRCodeList();
    }
}
