using AuthenticationServer.DataObjects;
using AuthenticationServer.Models;
using AuthenticationServer.ServiceContracts;
using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using DYFramework.ServiceContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Application
{
    public class ManagerService : Service<ManagerDataObject, Manager>, IManagerService
    {
        public ManagerService(IRepository<Manager> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public ManagerDataObject CheckManager(string username, string password)
        {
            var manager = this.repository.Get(p => (p.Email == username || p.UserName == username) && p.Password == password).FirstOrDefault();
            return mapper.Map<Manager, ManagerDataObject>(manager);
        }

    }
}
