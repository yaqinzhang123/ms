using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class UserInfo:AggregateRoot
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public virtual AppInfo AppInfo { get; set; }
        public virtual int AppInfoID { get; set; }
        public virtual ICollection<UserRole> UserRoleList { get; set; }
    }
}
