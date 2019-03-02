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
    public class UserRoleRepository:Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<UserRole> Get(Expression<Func<UserRole, bool>> expression)
        {
            return base.Get(expression).Include(p => p.UserInfo).Include(p => p.Role);
        }
    }
}
