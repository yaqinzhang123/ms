using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class Manager:AggregateRoot
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public virtual ICollection<AppInfo> AppList { get; set; }

        public Manager()
        {
            this.AppList = new List<AppInfo>();
        }
    }
}
