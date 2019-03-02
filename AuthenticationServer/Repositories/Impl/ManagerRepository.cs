using AuthenticationServer.Models;
using DYFramework.Domain;
using DYFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Repositories.Impl
{
    public class ManagerRepository : Repository<Manager>, IRepository<Manager>
    {
        public ManagerRepository(IRepositoryContext context) : base(context)
        {
        }

    }
}
