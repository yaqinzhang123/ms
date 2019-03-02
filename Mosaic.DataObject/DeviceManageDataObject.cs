using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class DeviceManageDataObject:DataObject
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public string ManageUrl { get; set; }
        public string Memo { get; set; }
        public int CompanyID { get; set; }
        public virtual CompanyDataObject Company { get; set; }

    }
}
