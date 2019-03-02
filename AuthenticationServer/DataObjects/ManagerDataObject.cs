using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DataObjects
{
    public class ManagerDataObject:DataObject
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public ICollection<int> AppIDList { get; set; }

    }
}
