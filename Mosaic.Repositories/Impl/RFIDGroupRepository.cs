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

    public class RFIDGroupRepository : Repository<RFIDGroup>, IRFIDGroupRepository
    {
        public RFIDGroupRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<RFIDGroup> Get(Expression<Func<RFIDGroup, bool>> expression)
        {
            return base.Get(expression);
        }
    }
}
