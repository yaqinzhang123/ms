using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class UserRole : AggregateRoot
    {
        public  int UserInfoID { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
    }
}
