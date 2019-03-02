using DYFramework.ServiceContract;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public interface IGroupService : IService<GroupDataObject>
    {
        void AddGroup(int sum,QRCodeDataObject qrcode);
        GroupDataObject GetGroupByQRCode(string qrcode);
        void UpdateRFID(RFIDRecordDataObject rfid, int i);
        bool ManualAddGroup(GroupDataObject group);
        int UpdateRFIDList(CarRFIDReceiverDataObject car);
        
    }
}
