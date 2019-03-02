using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RoleDataObject:DataObject
    {
        public string Name { get; set; }
        public int CompanyID { get; set; }
        public IList<RightsDataObject> RightsList { get; set; }
    }
}
