using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class SoftWare:AggregateRoot
    {
        public string Name { get; set; }
        public string Flag { get; set; }//公司Code对应
    }
}
