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
    public class RoleRepository:Repository<Role>, IRoleRepository
    {
        public RoleRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<Role> Get(Expression<Func<Role, bool>> expression)
        {
            return base.Get(expression).Include(p => p.rightsList);
        }
    }
}
