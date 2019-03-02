using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class LocationLogDataObject:DataObject
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public int ProductionLineID { get; set; }
    }
}
