using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class UserInfo:AggregateRoot
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string JobNumber { get; set; }
        public string TrueName { get; set; }
        public string Tel { get; set; }
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        //public UserInfo()
        //{
        //    this.RoleList = new List<Role>();
        //}
    }
}
