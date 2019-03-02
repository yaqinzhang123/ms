using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RFIDGroupDataObject:DataObject
    {
        public string PlateNumber { get; set; }
        public int OldInvoiceShipmentID { get; set; }
        public string OldInvoiceShipmentNo { get; set; }
        public int OldInvoiceID { get; set; }//旧单号
        public string OldInvoiceNo { get; set; }
        public IList<string> GroupNoList { get; set; }
        public IList<GroupDataObject> GroupList { get; set; }
        public IList<string> QRCodeList { get; set; }
        public DateTime OldTime { get; set; }
        public int CompanyID { get; set; }
        public virtual CompanyDataObject Company { get; set; }
    }
}
