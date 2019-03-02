using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Resolve
{
    public static class ArrayExtension
    {
        public static int Find(this string[] strArr,string str)
        {
            int result = -1;
            int index = 0;
            foreach(var item in strArr)
            {
                if(item.Contains(str))
                {
                    result = index;
                    break;
                }
                index++;
            }
            return result;
        }
    }
}
