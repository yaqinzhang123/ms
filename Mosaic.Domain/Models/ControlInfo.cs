using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class ControlInfo: AggregateRoot
    {
        public bool DI1 { get; set; }
        public bool DI2 { get; set; }
        public bool DI3 { get; set; }
        public bool DI4 { get; set; }
        public bool DI5 { get; set; }
        public bool DI6 { get; set; }
        public bool DI7 { get; set; }
        public bool DI8 { get; set; }
        public int ProductionLineID { get; set; }
        public bool Sync { get; set; }
        public bool Flag { get; set; }
        public DateTime Time { get; internal set; }

    }
}