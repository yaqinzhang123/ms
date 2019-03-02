using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Group:AggregateRoot
    {
       // public virtual List<QRCode> QRCodeList { get; set; }
        public DateTime Time { get; set; }
        public string RFID { get; set; }
        public string No {
            get { return this.ID.ToString(); }
        }
        public string Location { get; set; }
        public int ProductionLineID { get; set; }
        public string InvoiceNo { get; set; }
        public string RFIDGroupNo { get; set; }
        public bool Flag { get; set; }
        public int CID { get; set; }
    }
}
