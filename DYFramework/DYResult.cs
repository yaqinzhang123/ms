using System;
using System.Collections.Generic;
using System.Text;

namespace DYFramework
{
    public class DyResult
    {
        public DyStatusCode Code { get; set; }
        public object Data { get; set; }

        public DyResult(object obj,DyStatusCode code)
        {
            this.Data = obj;
            this.Code = code;
        }
        public DyResult(object obj)
        {
            this.Data = obj;
            this.Code = obj != null ? DyStatusCode.Success : DyStatusCode.Fail;
        }
    }
}
