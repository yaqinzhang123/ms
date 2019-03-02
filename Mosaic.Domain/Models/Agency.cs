using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class Agency : AggregateRoot
    {
       public string Name { get; set; }
       public string Zone { get; set; }
       public string Tel { get; set; }
        public string Contact { get; set; }//联系人
        public string ContactTel { get; set; }
        public string Grade { get; set; }
        public string SuperAgency { get; set; }
        public string Address { get; set; }
        public int CompanyID { get; set; }
    }
}
