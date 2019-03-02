using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.DTO
{
    public class GroupDataObject : DataObject
    {
        public virtual IList<QRCodeDataObject> QRCodeList { get; set; }
        public long Time { get; set; }
        public string RFID { get; set; }
        public int ProductionLineID { get; set; }
        public string No { get; set; }
        public string InvoiceNo { get; set; }
        public string RFIDGroupNo { get; set; }
        public string MaterialNo { get; set; }//物料编号
        public string Describe { get; set; }//描述
        public int CID { get; set; }
        public virtual CategoryDataObject Category { get; set; }
        public float Rule { get; set; }
        public bool Flag { get; set; }
        public int UserInfoID { get; set; }
        public string UserInfoName { get; set; }
        public OperationDataObject Operation { get; set; }
        public string Location { get; set; }
        public int Sum { get; set; }
    }
}


