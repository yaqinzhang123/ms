using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class CarInfo:AggregateRoot
    {
        public DateTime TriggerTime { get; set; }
        //public DateTime Leave { get; set; }
        public int ProductionLineID { get; set; }
        public string Location { get; set; }
        public bool Flag { get; set; }
        public bool Status { get; set; }
    }
}