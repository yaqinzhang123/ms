using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class CarInfoDataObject:DataObject
    {
        public DateTime TriggerTime { get; set; }
       // public DateTime Leave { get; set; }
        public int ProductionLineID { get; set; }
        public string Location { get; set; }
        public bool Flag { get; set; }
        public bool Status { get; set; }
        public CarInfoDataObject() : this(DateTime.Now)
        {

        }
        public CarInfoDataObject(DateTime enter)
        {
            TriggerTime = enter;
           // Leave = leave;
        }

        //public bool TimeIsUp(DateTime readTime, int extendedTime)
        //{
        //    return readTime > this.Leave.AddSeconds(extendedTime);
        //}
    }
}
