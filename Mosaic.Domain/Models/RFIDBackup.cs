using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class RFIDBackup:AggregateRoot
    {
        public string CodeList { get; set; }
        public int InvoiceID { get; set; }
    }
}
