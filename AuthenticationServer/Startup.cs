using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServer.Application;
using AuthenticationServer.DAO;
using AuthenticationServer.Models;
using AuthenticationServer.Repositories.Impl;
using AuthenticationServer.ServiceContracts;
using AuthenticationServer.Utils;
using AutoMapper;
using DYFramework.Dao;
using DYFramework.Domain;
using DYFramework.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AuthenticationServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            services.AddDbContext<DyContext, AuthContext>(options =>
              options.UseMySql(Configuration.GetConnectionString("mysql"), p => p.UnicodeCharSet(CharSet.Utf8mb3)));
            //options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), b => b.UseRowNumberForPaging()));

            services.AddDyService();

            services.AddAutoMapper();
            services.AddSession();
            services.AddMvc();
            services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(Config.GetIdentityResourceResources())
            .AddInMemoryApiResources(Config.GetApiResource())
            .AddInMemoryClients(Config.GetClients())
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddProfileService<ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCors(builder =>
                        builder.WithOrigins("*")
                                .WithHeaders("*")
                                .WithMethods("*"));
            app.UseStaticFiles();
            app.UseSession();
            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Manager}/{action=Index}/{id?}");
            });
        }
    }
}
