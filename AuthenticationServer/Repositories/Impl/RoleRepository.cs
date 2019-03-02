using AuthenticationServer.Models;
using DYFramework.Domain;
using DYFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Repositories.Impl
{
    public class RoleRepository : Repository<Role>, IRepository<Role>
    {
        public RoleRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}
