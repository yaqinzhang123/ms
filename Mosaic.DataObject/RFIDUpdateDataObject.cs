using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RFIDUpdateDataObject:DataObject
    {
        public QRCodeDataObject FirstQR { get; set; }
        public QRCodeDataObject EndQR { get; set; }
        public DateTime FirstQRTime { get; set; }
        public DateTime EndQRTime { get; set; }
        public CategoryDataObject Category { get; set; }
        public int QRSum { get; set; }
        public CarRFIDReceiverDataObject CarRFIDReceiverDataObject { get; set; }
    }
}
