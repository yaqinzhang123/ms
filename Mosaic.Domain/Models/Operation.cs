using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Operation:AggregateRoot
    {
        public virtual int CategoryID { get; set; }
        public string  Time { get; set; }//时间
        public string BatchNo { get; set; }//批号
        public int ProductionLineID { get; set; }//生产线
        public virtual ProductionLine ProductionLine { get; set; }
        public int GroupingMethod { get; set; }//分组方式 0:按固定数量分组  1：按重量分组
        public int Weight { get; set; }
        public int Sum { get; set; }
        public virtual int UserInfoID { get; set; }
        public bool State { get; set; }//状态
        public virtual int CompanyID { get; set; }
        public int Rule { get; set; }
        public double ReadRate { get; set; }//读码率预值
        public Operation()
        {
            this.GroupingMethod = 0;
            this.Weight = 2000;
        }
    }
}
