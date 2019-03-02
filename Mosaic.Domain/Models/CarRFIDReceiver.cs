using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class CarRFIDReceiver:AggregateRoot
    {
        public string RFIDList { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public string Location { get; set; }
    }
}
