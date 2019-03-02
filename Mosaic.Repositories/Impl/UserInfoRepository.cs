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
    public class UserInfoRepository:Repository<UserInfo>,IUserInfoRepository
    {
        public UserInfoRepository(IRepositoryContext context) : base(context)
        {
        }
        public override IQueryable<UserInfo> Get(Expression<Func<UserInfo, bool>> expression)
        {
            return base.Get(expression).Include(p => p.Company);
        }
    }
}
