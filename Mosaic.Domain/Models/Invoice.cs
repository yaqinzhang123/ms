using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Invoice : AggregateRoot
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
        public string img { get; set; }//交运单图片
        public bool Flag { get; set; }//标志
        public virtual IList<InvoiceShipment> InvoiceShipmentList { get; set; }
        public string GroupNoList { get; set; }//分组号表
        public string ErrGroupNoList { get; set; }//分组号表
        public string[] GroupNoArray { get { return this.GroupNoList?.Split(',').ToArray()??new string[] { }; } }
        public string CodeList { get; set; }//内容表
        public string ErrRFIDList { get; set; }//干扰
        public int CompanyID { get; set; }
        public int UserInfoID { get; set; }
        public bool Checked { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Memo { get; set; }
        public string LastGroupNoList { get; set; }//上一次组号
        public string ErrQRCodeList { get; set; }
        public string RemoveQRCodeList { get; set; }//去除的二维码
        public string RemoveGroupNoList { get; set; }//去除组号
        public string LastNo { get; set; }//最新的
    }
}