using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class CarInfoTransfer
    {
        public long TriggerTime { get; set; }
        //public long LeaveTime { get; set; }
        public int ProductionLineID { get; set; }
        public string Location { get; set; }
        public bool Status { get; set; }//线圈状态
        public bool Flag { get; set; }
        
    }
}
