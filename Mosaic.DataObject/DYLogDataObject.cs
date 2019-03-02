using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class DYLogDataObject:DataObject
    {
        public virtual UserInfoDataObject  UserInfo { get; set; }
        public string Key { get; set; }
        public string Memo { get; set; }
    }
}
