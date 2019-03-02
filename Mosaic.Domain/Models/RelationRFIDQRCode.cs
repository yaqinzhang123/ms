using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class RelationRFIDQRCode : AggregateRoot
    {
        public string Content { get; set; }//二维码内容
        public DateTime TimeQRCode { get; set; }//扫描时间
        public string RFID { get; set; }//RFID
        public DateTime TimeRFID { get; set; }//RFID扫描时间
        public UserInfo userInfo { get; set; }
    }
}
