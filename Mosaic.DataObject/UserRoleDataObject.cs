using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class UserRoleDataObject : DataObject
    {
        public int UserInfoID { get; set; }
        public virtual UserInfoDataObject UserInfo { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public virtual RoleDataObject Role { get; set; }
       
    }
}
