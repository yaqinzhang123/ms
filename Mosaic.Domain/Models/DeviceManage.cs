using DYFramework.DataObjects;
using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class DeviceManage : AggregateRoot
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public string ManageUrl { get; set; }
        public string Memo { get; set; }
        public int CompanyID { get; set; }
    }
}
