using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class RFIDRecord:AggregateRoot
    {
       // public int ID { get; set; }
        public string RFID { get; set; }
        public int LineID { get; set; }
        public DateTime Time { get; set; }
        public bool Sync { get; set; }
        public int Times { get; set; }
        public bool Flag { get; set; }
        public string Location { get; set; }


        
    }
}
