using AuthenticationServer.Models;
using DYFramework.Domain;
using DYFramework.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthenticationServer.Repositories.Impl
{
    public class UserInfoRepository : Repository<UserInfo>, IRepository<UserInfo>
    {
        public UserInfoRepository(IRepositoryContext context) : base(context)
        {

        }
        public override IQueryable<UserInfo> Get(Expression<Func<UserInfo, bool>> expression)
        {
            return this.Context.GetReadEntity<UserInfo>().Include(p => p.AppInfo).Where(expression);
        }

    }
}
