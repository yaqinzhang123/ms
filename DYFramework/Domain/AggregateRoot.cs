using System;
using System.Collections.Generic;
using System.Text;

namespace DYFramework.Domain
{
    public class AggregateRoot
    {
        public int ID { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
