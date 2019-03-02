using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class VirtualPrinterDataDataObject:DataObject
    {
        public string PSFileContent { get; set; }
        public string TxtFileContent { get; set; }
        public DateTime PrintTime { get; set; }
        public int CompanyID { get; set; }
        public bool Flag { get; set; }
    }
}
