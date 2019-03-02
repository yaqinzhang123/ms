using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class CarNoForRFID:AggregateRoot
    {
        public string CarNo { get; set; }
        public string RFID { get; set; }
    }
}
