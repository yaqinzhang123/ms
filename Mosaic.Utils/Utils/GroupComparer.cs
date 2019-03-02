using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Mosaic.Utils.Utils
{
    public class GroupComparer : IEqualityComparer<Group>
    {
        public bool Equals(Group x, Group y)
        {
            return x.ID==y.ID;
        }

        public int GetHashCode(Group obj)
        {
            return this.GetHashCode();
        }
    }
}
