using DYFramework.Domain;
using DYFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Mosaic.Domain.Models;
using Mosaic.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mosaic.Repositories.Impl
{
    public class RightsRepository:Repository<Rights>, IRightsRepository
    {
        public RightsRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<Rights> Get(Expression<Func<Rights, bool>> expression)
        {
            return base.Get(expression).Include(p => p.Module);
        }
    }
}
