using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class CarDataTransfer
    {
        public int Line { get; set; }
        public string Location { get; set; }
        public string RFID1 { get; set; }
        public string RFID2 { get; set; }
        public string RFID3 { get; set; }
        public long Enter { get; set; }
        public long Leave { get; set; }
        public dynamic Duration { get { return Leave - Enter; } }
        public bool Uploaded { get; set; }
        public bool SecondFilterd { get; set; }
    }
}
