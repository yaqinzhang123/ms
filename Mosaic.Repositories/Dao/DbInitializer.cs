using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.Dao
{
    public class DbInitializer
    {
        public static void Initialize(MosaicContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
