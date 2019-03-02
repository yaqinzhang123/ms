using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RFIDRecordDataObject:DataObject
    {
        public string RFID { get; set; }
        public int LineID { get; set; }
        public DateTime Time { get; set; }
        public bool Sync { get; set; }
        public int Times { get; set; }
        public bool Flag { get; set; }
        public string Location { get; set; }

        private int CUTOFF_MIN = 20;

        public bool IsMinutesAgo()
        {
            return (DateTime.Now - this.Time).TotalMilliseconds > this.CUTOFF_MIN * 60 * 1000;
        }

        public bool Between(DateTime enter, DateTime leave)
        {
            return this.Time >= enter && this.Time <= leave;
        }

        public bool BeforeExtendedTime(DateTime leave)
        {
            return this.Time < leave.AddSeconds(60);
        }


    }
}
