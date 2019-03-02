﻿using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class InvoiceDataObject : DataObject
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
        public bool Flag { get; set; }
        public virtual IList<InvoiceShipmentDataObject> InvoiceShipmentList { get; set; }//装运信息
        public IList<string> GroupNoList { get; set; }//分组表
        public IList<string> CodeList { get; set; }//二维码内容表
        public IList<string> RFIDList { get; set; }//RFID干扰

        public int Page { get; set; }
        public virtual CompanyDataObject Company { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyCode { get; set; }//公司代码
        public string CompanyFax { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyRegistrationCode { get; set; }
        public IList<string> ProductionLineName { get; set; }
        public virtual IList<CategoryDataObject> CategoryList { get; set; }
        public virtual IList<GroupDataObject> GroupList { get; set; }
        public virtual IList<GroupDataObject> ErrGroupList { get; set; }
        public IList<string> ErrRFIDList { get; set; }
        public IList<string> ErrGroupNoList { get; set; }
        public int UserInfoID { get; set; }
        public UserInfoDataObject UserInfo { get; set; }
        // public RFIDGroupDi RFIDGroupDi { get; set; }
        public bool Checked { get; set; }
        public string SubmitTime { get; set; }
        public IList<string> LastGroupNoList { get; set; }//上一次组号
        public IList<string> ErrQRCodeList { get; set; }
        public IList<string> RemoveQRCodeList { get; set; }//去除的二维码
        public IList<string> RemoveGroupNoList { get; set; }//去除组号
        public string LastNo { get; set; }//最新的
        public int NewID { get; set; }
        public string RevokeQRCode { get; set; }
        public IList<InvoiceUserInfoDataObject> InvoiceUserInfoList { get; set; }
        public int CID { get; set; }
    }
}