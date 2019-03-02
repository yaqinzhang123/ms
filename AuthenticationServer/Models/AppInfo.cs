using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class AppInfo:AggregateRoot
    {
        public string AppId { get; set; }
        public string AppName { get; set; }
        public virtual Manager Manager { get; set; }
        public int ManagerID { get; set; }
        public virtual ICollection<UserInfo> UserList { get; set; }


        public AppInfo()
        {
            this.UserList = new List<UserInfo>();
        }

    }
}
