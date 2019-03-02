using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class QRCodeDataObject : DataObject
    {
        public string Content { get; set; }//二维码内容
        public long Time { get; set; }//扫描时间
        public DateTime ScanTime { get
            {
                return DateTime.MinValue.AddTicks(this.Time);
            } }
        public int ProductionLineID { get; set; }
        public virtual ProductionLineDataObject ProductionLine { get; set; }
        public int OperationID { get; set; }
        public virtual OperationDataObject Operation { get; set; }
        public virtual GroupDataObject Group { get; set; }
        public virtual InvoiceDataObject Invoice { get; set; }
        public virtual CompanyDataObject Company { get; set; }
        public int CompanyID { get; set; }
        public virtual CategoryDataObject Category { get; set; }
        public int OperationRule { get; set; }
        public int GID { get; set; }
        public int CID { get; set; }
        public bool StartRoot { get; set; }//开始节点
        public bool EndRoot { get; set; }//结束节点
        public bool AutoSkip { get; set; }//自动跳过
        public bool ManualSkip { get; set; }//手动跳过
        public int VirtualAdd { get; set; }//虚增
        public bool Lock { get; set; }//是否锁定
        public int Index { get; set; }
        public int GroupSum { get; set; }
        public int GNO { get; set; }
    }
}
