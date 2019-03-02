using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    //触发器表
    public class LocationLog:AggregateRoot
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public int ProductionLineID { get; set; }
    }
}
