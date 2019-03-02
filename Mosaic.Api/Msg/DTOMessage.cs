using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosaic.Api.Msg
{
    public class DTOMessage<T> : DyMessage
    {
        public T Data { get; set; }
    }
}
