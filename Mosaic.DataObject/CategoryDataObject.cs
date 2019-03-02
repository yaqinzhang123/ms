using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class CategoryDataObject:DataObject
    {
        public string MaterialNo { get; set; }//物料编号
        public string Describe { get; set; }//描述
        public string Manufacturer { get; set; }//生产商
        public int ManufacturerNo { get; set; }//生产商代码
        public string Yieldly { get; set; }//生产地
        public string Img { get; set; }//图片
        public int Page { get; set; }
        public int CompanyID { get; set; }//公司id
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
