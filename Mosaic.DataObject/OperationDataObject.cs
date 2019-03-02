using DYFramework.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class OperationDataObject:DataObject
    {
        public int CategoryID { get; set; }
        public string Time { get; set; }//时间
        public string BatchNo { get; set; }//批号 
        public int ProductionLineID { get; set; }//生产线
        public string ProductionLineName { get; set; }
        public int Sum { get; set; }
        public int UserInfoID { get; set; }
        public int PageNo { get; set; }
        public int Rule { get; set; }
        public int CompanyID { get; set; }
        public virtual CategoryDataObject Category { get; set; }
        public bool State { get; set; }
        public int GroupingMethod { get; set; }//分组方式 0:按固定数量分组  1：按重量分组
        public int Weight { get; set; }
        public string CategoryMaterialNo { get; set; }
        public string GroupState { get; set; }
        public double ReadRate { get; set; }//读码率预值
        public bool Flag { get; set; }
    }
}
