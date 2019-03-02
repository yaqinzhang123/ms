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

    public class OperationRepository : Repository<Operation>, IOperationRepository
    {
        public OperationRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<Operation> Get(Expression<Func<Operation, bool>> expression)
        {
            return base.Get(expression).Include(p=>p.ProductionLine);
        }
    }
}
