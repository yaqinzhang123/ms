using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class InvoiceUserInfo:AggregateRoot
    {
        public int InvoiceID { get; set; }
        public int UserInfoID { get; set; }
        public string UserInfoName { get; set; }
        public string GroupNoList { get; set; }
        public string CodeList { get; set; }
        public string[] GroupNoArray { get { return this.GroupNoList?.Split(',').ToArray() ?? new string[] { }; } }
    }
}
