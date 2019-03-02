using DYFramework.DataObjects;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class InvoiceShipmentDataObject : DataObject
    {
        public int InvoiceID { get; set; }
        public string InvoiceNo { get; set; }//交货单号
        public string PlateNumber { get; set; }//车牌号
        public string DriverName { get; set; }//司机名
        public string DriverPhoneNo { get; set; }//司机电话
        public string Project { get; set; }//装运项目
        public string MaterialNo { get; set; }//装运物料
        public double Quantity { get; set; }//装运数量
        public string Describe { get; set; }//装运物料描述
        public IList<string> GroupNoList { get; set; }
        public int CID { get; set; }
        public int QRRule { get; set; }
        public int GroupSum { get; set; }//分组数量
        public int GroupTrue {//实发兜数
            get
            {
                return GroupNoList==null?0: GroupNoList.Count;
            } }
        public int QRCount { get; set; }
        public double GroupQuantitySum
        {
            get
            {
                return (QRRule * GroupSum / 1000.0);
            }
        }
        public bool Flag
        {
            get
            {
                return GroupQuantitySum >= Quantity;
            }
        }
    }
}