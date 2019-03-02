using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RelationRFIDQRCodeDataObject : DataObject
    {
        public string Content { get; set; }//二维码内容
        public DateTime TimeQRCode { get; set; }//扫描时间
        public string RFID { get; set; }//RFID
        public DateTime TimeRFID { get; set; }//RFID扫描时间
        public UserInfoDataObject userInfo { get; set; }
    }
}
