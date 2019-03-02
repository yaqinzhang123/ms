using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Module : AggregateRoot
    {
        public string Name { get; set; }
        public virtual SoftWare SoftWare { get; set; }
    }
}
