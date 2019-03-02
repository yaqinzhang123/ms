using AuthenticationServer.DataObjects;
using AuthenticationServer.Models;
using AuthenticationServer.ServiceContracts;
using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using DYFramework.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Application
{
    public class RoleService : Service<RoleDataObject, Role>, IRoleService
    {
        public RoleService(IRepository<Role> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
