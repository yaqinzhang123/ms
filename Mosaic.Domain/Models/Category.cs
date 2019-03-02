using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Category: AggregateRoot
    {
        public string MaterialNo { get; set; }//物料编号
        public string Describe { get; set; }//描述
        public string Manufacturer { get; set; }//生产商
        public int ManufacturerNo { get; set; }//生产商代码
        public string Yieldly { get; set; }//生产地
        public string Img { get; set; }//图片
        public int CompanyID { get; set; }//公司id
        public virtual Company Company { get; set; }
    }
}
