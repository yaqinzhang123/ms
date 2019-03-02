using AutoMapper;
using Autofac;
using DYFramework.Dao;
using DYFramework.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mosaic.Api.Models;
using Mosaic.Application.Impl;
using Mosaic.Domain.Models;
using Mosaic.Repositories.Dao;
using Mosaic.Repositories.Impl;
using Mosaic.ServiceContracts;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DYFramework;
using System.Collections.Generic;
using System.Linq;
using Mosaic.Domain.Repository;
using Mosaic.SingletonService;
using log4net.Repository;
using log4net;
using log4net.Config;
using System.IO;

namespace Mosaic.Api
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            services.AddDbContext<DyContext, MosaicContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), b => b.UseRowNumberForPaging()));
            services.AddScoped<IRepositoryContext, DyRepositoryContext>();
            IConfigurationSection injectionConfig = Configuration.GetSection("InjectionConfig");

            string domain = injectionConfig.GetSection("Domain").Value;
            string repo = injectionConfig.GetSection("Repository").Value;
            services.RegisterAssembly(domain, repo);

            string serviceContract = injectionConfig.GetSection("ServiceContracts").Value;
            string app = injectionConfig.GetSection("Application").Value;
            services.RegisterAssembly(serviceContract, app);
            services.AddAutoMapper();
            services.AddLogging();
            //services.AddSingleton<IRFIDService, RFIDService>();
            services.AddSingleton<GroupingService>();
            services.AddSingleton<RFIDRelationService>();
            services.AddSingleton<FragmentService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationBuilder applicationBuilder,
            IModuleRepository moduleRepository, ISoftWareRepository softWareRepository, DyContext context,
            IRoleRepository roleRepository, ICompanyRepository companyRepository, IUserInfoRepository userInfoRepository,
            IUserRoleRepository userRoleRepository, IRightsRepository rightsRepository,
            GroupingService groupingService,RFIDRelationService rfidRelationService,
            FragmentService fragmentService)
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
                builder.WithHeaders("*")
                .WithMethods("*")
                .WithOrigins("*"));


            context.Database.Migrate();


            if (moduleRepository.GetCount() <= 0)
            {
                List<Domain.Models.Module> moduleList = new List<Domain.Models.Module>
                    {       new Domain.Models.Module() { Name="厂区管理"},
                            new Domain.Models.Module() { Name="产线管理"},
                            new Domain.Models.Module() { Name="产品管理"},
                            new Domain.Models.Module() { Name="工业PC"},
                            new Domain.Models.Module() { Name="手持扫码设备"},
                            new Domain.Models.Module() { Name="角色管理"},
                            new Domain.Models.Module() { Name="人员管理"},
                            new Domain.Models.Module() { Name="一级经销商"},
                            new Domain.Models.Module() { Name="二级经销商"},
                            new Domain.Models.Module() { Name="三级经销商"},
                            new Domain.Models.Module() { Name="VIP用户管理"}
                    };
                moduleList.ForEach((item) =>
                {
                    moduleRepository.Add(item);
                });
                moduleRepository.Commit();
            }
            if (softWareRepository.GetCount() <= 0)
            {
                List<Domain.Models.SoftWare> softWareList = new List<Domain.Models.SoftWare>
                    {       new Domain.Models.SoftWare() { Name="多工厂云平台"},
                            new Domain.Models.SoftWare() { Name="生产品类设置软件"},
                            new Domain.Models.SoftWare() { Name="储运员数据管理软件"},
                            new Domain.Models.SoftWare() { Name="装车发运软件（秦皇岛）",Flag="5519"},
                            new Domain.Models.SoftWare() { Name="装车发运软件（烟台）",Flag="5518"},
                            new Domain.Models.SoftWare() { Name="追溯查询软件"},
                    };
                softWareList.ForEach((item) =>
                {
                    softWareRepository.Add(item);
                });
                softWareRepository.Commit();
            }

            if (companyRepository.GetCount() <= 0)
            {
                List<Company> companyList = new List<Company>
                    {
                    new Domain.Models.Company() {
                        Name="美盛农资（北京）有限公司",
                        SoftList ="多工厂云平台,生产品类设置软件,储运员数据管理软件,装车发运软件（秦皇岛）,装车发运软件（烟台）,追溯查询软件"
                    }
                    };
                companyList.ForEach((item) =>
                {
                    companyRepository.Add(item);
                });
                companyRepository.Commit();
            }
            if (userInfoRepository.GetCount() <= 0)
            {
                List<UserInfo> userInfoList = new List<UserInfo>
                    {
                    new Domain.Models.UserInfo() {Name="super",Password="super123",CompanyID=1
                    }
                    };
                userInfoList.ForEach((item) =>
                {
                    userInfoRepository.Add(item);
                });
                userInfoRepository.Commit();
            }

            if (roleRepository.GetCount() <= 0)
            {
                List<Role> roleList = new List<Role>
                    {
                    new Domain.Models.Role() { Name="超级管理员",CompanyID=1}
                    };
                roleList.ForEach((item) =>
                {
                    roleRepository.Add(item);
                });
                roleRepository.Commit();
            }

            if (userRoleRepository.GetCount() <= 0)
            {
                List<UserRole> userInfoList = new List<UserRole>
                    {
                    new Domain.Models.UserRole() {UserInfoID=1,RoleID=1}
                    };
                userInfoList.ForEach((item) =>
                {
                    userRoleRepository.Add(item);
                });
                userRoleRepository.Commit();
            }
            if (rightsRepository.GetCount() <= 0)
            {
                List<Rights> rightsList = new List<Rights>
                    {
                    new Domain.Models.Rights() {SoftName="多工厂云平台",RoleID=1,Factory="美盛农资（北京）有限公司",FactoryID=1}
                    };
                rightsList.ForEach((item) =>
                {
                    rightsRepository.Add(item);
                });
                rightsRepository.Commit();
            }
           // rFIDService.Start();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });
            app.UseStaticFiles();
            //app.UseDefaultFiles( );
            app.UseMvc(route=>
            {
                route.MapRoute(
                    name:"default",
                    template:"{controller=Home}/{action=Index}/{id?}"
                    );
            });
            if(env.IsProduction())
            groupingService.Start();
            //rfidRelationService.Start();
            fragmentService.Start();
        }
    }
}
