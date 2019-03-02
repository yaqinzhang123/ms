using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class ProductionLine:AggregateRoot
    {
        public virtual Company Company { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string RFIDDevice { get; set; }
       // public virtual IList<Operation> OperationList { get; set; }
        public int OperationID { get; set; }
        public bool Flag { get; set; }
    }
}
