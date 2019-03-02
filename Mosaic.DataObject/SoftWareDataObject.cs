using DYFramework.DataObjects;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class SoftWareDataObject:DataObject
    {
        public string Name { get; set; }
        public string Flag { get; set; }//公司Code对应
    }
}
