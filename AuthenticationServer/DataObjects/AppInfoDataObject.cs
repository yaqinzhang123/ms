using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DataObjects
{
    public class AppInfoDataObject:DataObject
    {
        public string AppID { get; set; }
        public string AppName { get; set; }
        public ICollection<int> UserIDList { get; set; }
    }
}
