using DYFramework.Domain;
using DYFramework.Repository;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.Impl
{
    public class AgencyRepository:Repository<Agency>, IAgencyRepository
    {
        public AgencyRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}
