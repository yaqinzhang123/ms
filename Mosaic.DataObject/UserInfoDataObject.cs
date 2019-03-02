using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class UserInfoDataObject:DataObject
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string JobNumber { get; set; }
        public string TrueName { get; set; }
        public string Tel { get; set; }
        public virtual IList<UserRoleDataObject> UserRole { get; set; }
        public virtual IList<RoleDataObject> Role { get; set; }
        public virtual IList<RightsDataObject> Rights { get; set; }
        public int CompanyID { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyCode { get; set; }//公司代码
        public string CompanyFax { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyRegistrationCode { get; set; }
    }
}
