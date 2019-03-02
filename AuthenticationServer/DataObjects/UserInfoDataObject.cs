using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DataObjects
{
    public class UserInfoDataObject:DataObject
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string AppID { get; set; }
        public ICollection<int> RoleIDList { get; set; }
        
    }
}
