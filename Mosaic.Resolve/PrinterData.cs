using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Resolve
{
    public class PrinterData
    {
        public int ID { get; set; }
        public string PSFileContent { get; set; }
        public string TxtFileContent { get; set; }
        public DateTime PrintTime { get; set; }
        public int CompanyID { get; set; }
    }
}
