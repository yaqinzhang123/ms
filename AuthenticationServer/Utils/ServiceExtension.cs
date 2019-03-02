using AuthenticationServer.Application;
using AuthenticationServer.Models;
using AuthenticationServer.Repositories.Impl;
using AuthenticationServer.ServiceContracts;
using DYFramework.Dao;
using DYFramework.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Utils
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDyService(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryContext, DyRepositoryContext>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IRepository<UserInfo>, UserInfoRepository>();
            services.AddScoped<IRepository<AppInfo>, AppInfoRepository>();
            services.AddScoped<IAppInfoService, AppInfoService>();
            services.AddScoped<IRepository<Manager>, ManagerRepository>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IRepository<Role>, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            return services;
        }
    }
}
