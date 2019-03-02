using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class InvoiceUserInfoDataObject : DataObject
    {
        public int InvoiceID { get; set; }
        public int UserInfoID { get; set; }
        public IList<string> GroupNoList { get; set; }
        public IList<string> CodeList { get; set; }
        public string UserInfoName { get; set; }
    }
}
