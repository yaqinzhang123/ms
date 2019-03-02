using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Role:AggregateRoot
    {
        public string Name { get; set; }
        public int CompanyID { get; set; }
        //public string ABC { get; set; }
        public virtual List<Rights> rightsList { get; set; }
    }
}
