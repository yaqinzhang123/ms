using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class CarRFIDReceiverDataObject:DataObject
    {
        public string RFIDList { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public string Location { get; set; }
        public int ProductionLineID { get; set; }
    }
}
