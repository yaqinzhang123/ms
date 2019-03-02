using AuthenticationServer.Models;
using AuthenticationServer.Models.ModelConfig;
using DYFramework.Dao;
using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DAO
{
    public class AuthContext : DyContext
    {
        public AuthContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void AddTypeConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new UserInfoConfig())
                .ApplyConfiguration(new TypeConfiguration<AppInfo>())
                .ApplyConfiguration(new TypeConfiguration<Manager>())
                .ApplyConfiguration(new RoleConfig());
            

            
        }

    }
}
