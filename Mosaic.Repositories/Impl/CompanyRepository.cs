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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<Company> Get(Expression<Func<Company, bool>> expression)
        {
            return base.Get(expression).Include(p=>p.ProductionLineList);
        }
    }
}
