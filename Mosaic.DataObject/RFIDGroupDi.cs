using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class RFIDGroupDi
    {
        public Dictionary<string, IList<GroupDataObject>> ErrorGroupList { get; set; }
        public Dictionary<string,IList<GroupDataObject>> TrueGroupList { get; set; }
    }
}
