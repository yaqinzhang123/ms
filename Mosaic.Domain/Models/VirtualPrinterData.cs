using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class VirtualPrinterData:AggregateRoot
    {
        public string PSFileContent { get; set; }
        public string TxtFileContent { get; set; }
        public DateTime PrintTime { get; set; }
        public int CompanyID { get; set; }
        public bool Flag { get; set; }
    }
}
