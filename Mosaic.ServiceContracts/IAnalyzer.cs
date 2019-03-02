
using Mosaic.Domain.Models;
using Mosaic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.ServiceContracts
{
    public delegate void SelectedRFID(RFIDRecordDataObject rfid,CarInfoDataObject carInfo);
    public interface IAnalyzer
    {
        event SelectedRFID OnSelectedRFID;
        List<string> RFIDTag();
        void AddRecord(RFIDRecordDataObject record);
        void ShowRFIDInfo();
       // void LifterTrigger(CarInfoDataObject carInfo);

    }
}
