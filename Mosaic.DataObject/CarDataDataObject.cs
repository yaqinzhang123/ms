using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class CarDataDataObject:DataObject
    {
        public int Line { get; set; }
        public string Location { get; set; }
        public string RFID1 { get; set; }
        public string RFID2 { get; set; }
        public string RFID3 { get; set; }
        public string RFID
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.RFID3) ? string.IsNullOrWhiteSpace(this.RFID2) ? this.RFID1 : this.RFID2 : this.RFID3;
            }
        }
        public DateTime Enter { get; set; }
        public DateTime Leave { get; set; }
        public dynamic Duration { get { return Leave - Enter; } }
        public bool Uploaded { get; set; }
        public bool SecondFilterd { get; set; }
        public bool Handled { get; set; }

    }
}
