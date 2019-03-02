using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RFIDBackupDataObject:DataObject
    {
        public IList<string> CodeList { get; set; }
        public int InvoiceID { get; set; }
    }
}
