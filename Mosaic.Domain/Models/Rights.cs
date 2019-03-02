using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Rights:AggregateRoot
    {
        public virtual Module Module { get; set; }
        public string SoftName { get; set; }
        public int RoleID { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public string Factory  { get; set; }
        public int FactoryID { get; set; }
    }
}
