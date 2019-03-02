using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Mosaic.Utils.Utils
{
    public class StringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x == y;
        }

        public int GetHashCode(string  obj)
        {
            return this.GetHashCode();
        }
    }
}
