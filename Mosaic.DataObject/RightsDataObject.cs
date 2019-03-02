using DYFramework.DataObjects;
using Mosaic.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RightsDataObject:DataObject
    {
     
        public string SoftName { get; set; }
        public int RoleID { get; set; }
        public string Factory { get; set; }
        public int FactoryID { get; set; }
    }
}
