using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class UserRole:AggregateRoot
    {
        public virtual UserInfo UserInfo { get; set; }
        public virtual Role Role { get; set; }

    }
}
