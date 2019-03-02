using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Domain.Models
{
    public class DYLog:AggregateRoot
    {
        public string Key { get; set; }
        public string Memo { get; set; }
    }
}
