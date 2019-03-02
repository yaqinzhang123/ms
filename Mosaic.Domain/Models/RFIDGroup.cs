using DYFramework.Domain;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class RFIDGroup:AggregateRoot
    {
        public string PlateNumber { get; set; }
        public int OldInvoiceShipmentID { get; set; }//旧车号
        public string OldInvoiceShipmentNo { get; set; }//旧单号
        public string OldInvoiceNo { get; set; }//旧单号
        public int OldInvoiceID { get; set; }//旧单号
        public string GroupNoList { get; set; }//分组表
        public string QRCodeList { get; set; }
        public DateTime OldTime { get; set; }
        public int CompanyID{ get; set; }
    }
}
