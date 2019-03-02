using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class CompanyDataObject:DataObject
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostNo { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string RegistrationCode { get; set; }
        public string Code { get; set; }//公司代码
        public string ShortCode { get; set; }//简称代码 ：秦皇岛Q烟台Y
        public IList<ProductionLineDataObject> productionLineList { get; set; }
        public IList<string> SoftList { get; set; }
    }
}
