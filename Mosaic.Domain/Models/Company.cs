using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Company:AggregateRoot
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostNo { get; set; }//邮编
        public string Tel { get; set; }
        public string Fax { get; set; }//传真
        public string Email { get; set; }
        public string Contact { get; set; }//联系人
        public string RegistrationCode { get; set; }//注册码
        public string Code { get; set; }//公司代码
        public string ShortCode { get; set; }//简称代码 ：秦皇岛Q烟台Y
        public IList<ProductionLine> ProductionLineList { get; set; }
        public string SoftList { get; set; }

        public Company()
        {
            this.ProductionLineList = new List<ProductionLine>();
        }

    }
}
