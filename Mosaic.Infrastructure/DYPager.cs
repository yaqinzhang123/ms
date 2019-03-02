using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Infrastructure
{
    public class DYPager
    {
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public int Total { get; set; }
        public DYPager()
        {
            PageSize = 20;
            PageIndex = 1;
        }
    }
}
