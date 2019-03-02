using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class Role : AggregateRoot
    {
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoleList { get; set; }
    }
}
