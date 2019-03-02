using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class InvoiceAndShipment : DataObject
    {
        public string No { get; set; }//交货单号
        public string InvoiceTime { get; set; }//交货日期
        public string OrderNo { get; set; } //订单号
        public string OrderTime { get; set; }//订单日期
        public string CustomerOrderNo { get; set; } //订单号
        public string CustomerOrderTime { get; set; }//订单日期
        public string CustomerNo { get; set; }//客户编号
        public string BatchNumber { get; set; }//检验批次号
        public string DealerName { get; set; }//经销商名称
        public string DealerPostcord { get; set; }//经销商邮编
        public string DealerPlace { get; set; }//经销商地址
        public string ShipmentMode { get; set; }//装运方式
        public string DeliveryMode { get; set; }//交货方式
        public string PlateNumber { get; set; }//车牌号
        public string DriverName { get; set; }//司机名字
        public string DriverPhoneNo { get; set; }//司机电话
        public string Quantity { get; set; }//数量
        public string Memo { get; set; }
        public string Img { get; set; }//交运单图片
        public virtual IList<InvoiceShipmentDataObject> InvoiceShipmentList { get; set; }//装运信息
        public IList<string> GroupNoList { get; set; }//分组表
        public IList<string> CodeList { get; set; }//二维码内容表
                                                   //  public IList<string> RFIDList { get; set; }//RFID
        public int Page { get; set; }
        public int CompanyID { get; set; }

        public string Project { get; set; }//装运项目
        public string MaterialNo { get; set; }//装运物料
        public string Describe { get; set; }//装运物料描述
    }
}
