using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class ProductionLineDataObject:DataObject
    {
        public string Name { get; set; }
        public string RFIDDevice { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyCode { get; set; }//公司代码
        public string CompanyFax { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyRegistrationCode { get; set; }
        public virtual OperationDataObject Operation { get; set; }
        public virtual CategoryDataObject Category { get; set; }
        public int OperationID { get; set; }
        public bool Flag { get; set; }
    }
}
