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
    public class AppInfoRepository : Repository<AppInfo>, IRepository<AppInfo>
    {
        public AppInfoRepository(IRepositoryContext context) : base(context)
        {
        }

        public override IQueryable<AppInfo> GetAll()
        {

            return this.Context.GetAll<AppInfo>().Include(p => p.UserList);
        }
        public override IQueryable<AppInfo> Get(Expression<Func<AppInfo, bool>> expression)
        {
            return this.Context.GetAll<AppInfo>().Include(p => p.UserList).Where(expression);
        }
    }
}
