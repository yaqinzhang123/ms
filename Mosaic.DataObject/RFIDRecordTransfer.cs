using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RFIDRecordTransfer
    {
        public string RFID { get; set; }
        public int ProductionLineID { get; set; }
        public long Time { get; set; }
        public bool Sync { get; set; }
        public int Times { get; set; }
        public bool Flag { get; set; }
        public string Location { get; set; }

    }
}
