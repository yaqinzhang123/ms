using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class QRCode:AggregateRoot
    {
        public string Content { get; set; }//二维码内容
        public DateTime Time { get; set; }//扫描时间
        public  int OperationID { get; set; }
        public bool StartRoot { get; set; }//开始节点
        public bool EndRoot { get; set; }//结束节点
        public bool AutoSkip { get; set; }//自动跳过
        public bool ManualSkip { get; set; }//手动跳过
        public int VirtualAdd { get; set; }//虚增
        public bool Lock { get; set; }//是否锁定
        public int OperationRule { get; set; }
        public int  GID { get; set; }
        public int CID { get; set; }
        public int ProductionLineID { get; set; }
        public string Location { get; set; }
    }
}
