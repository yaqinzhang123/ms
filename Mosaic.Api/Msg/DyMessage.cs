using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic.Api.Msg
{
    public class DyMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public int PageCount { get; set; }
        public int PageNo { get; set; }
        public string Quantity { get;set; }
    }
}
