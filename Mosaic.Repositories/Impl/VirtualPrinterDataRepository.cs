using DYFramework.Domain;
using DYFramework.Repository;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.Impl
{
    public class VirtualPrinterDataRepository:Repository<VirtualPrinterData>, IVirtualPrinterDataRepository
    {
        public VirtualPrinterDataRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}
