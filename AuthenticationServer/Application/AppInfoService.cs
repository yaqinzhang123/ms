using AuthenticationServer.DataObjects;
using AuthenticationServer.Models;
using AuthenticationServer.ServiceContracts;
using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Application
{
    public class AppInfoService : Service<AppInfoDataObject, AppInfo>, IAppInfoService
    {
        public AppInfoService(IRepository<AppInfo> repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public override AppInfoDataObject Add(AppInfoDataObject dataObject)
        {
            AppInfo appinfo = this.repository.Create();
            appinfo = this.mapper.Map<AppInfoDataObject,AppInfo>(dataObject);
            var userlist = repository.Context.GetUpdateEntity<UserInfo>().Where(p => dataObject.UserIDList.Contains(p.ID));
            foreach(var item in userlist)
            {
                appinfo.UserList.Add(item);
            }
            repository.Add(appinfo);
            repository.Commit();
            return this.mapper.Map<AppInfo, AppInfoDataObject>(appinfo);
        }
        public override AppInfoDataObject Update(AppInfoDataObject dataObject)
        {
            AppInfo appinfo = this.repository.Context.GetUpdateEntity<AppInfo>().Where(p => p.ID == dataObject.ID).FirstOrDefault();
            appinfo = this.mapper.Map(dataObject,appinfo);
            var userlist = repository.Context.GetUpdateEntity<UserInfo>().Where(p => dataObject.UserIDList.Contains(p.ID));
            appinfo.UserList.Clear();
            foreach (var item in userlist)
            {
                appinfo.UserList.Add(item);
            }
            repository.Update(appinfo);
            repository.Commit();
            return this.mapper.Map<AppInfo, AppInfoDataObject>(appinfo);
        }
    }
}
