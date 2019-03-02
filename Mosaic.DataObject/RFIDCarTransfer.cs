using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RFIDCarTransfer:DataObject
    {
        public int Line { get; set; }
        public string Location { get; set; }
        public string RFID { get; set; }
        public long Enter { get; set; }
        public long Leave { get; set; }
        public dynamic Duration { get { return Leave - Enter; } }
    }
}
