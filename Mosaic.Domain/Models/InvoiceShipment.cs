using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class InvoiceShipment : AggregateRoot
    {
        public int InvoiceID { get; set; }
        public string PlateNumber { get; set; }//车号
        public string DriverName { get; set; }//司机名字
        public string DriverPhoneNo { get; set; }//司机电话
        public string Project { get; set; }//装运项目
        public string MaterialNo { get; set; }//装运物料
        public string Quantity {  get;set;  }//装运数量
        public string Describe { get; set; }//装运物料描述
        public string GroupNoList { get; set; }
        public int CID { get; set; }
        public int QRRule { get; set; }
        public int GroupSum { get; set; }//分组数量
       
    }
}