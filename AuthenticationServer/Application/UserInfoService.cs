using AuthenticationServer.DataObjects;
using AuthenticationServer.Models;
using AuthenticationServer.ServiceContracts;
using AutoMapper;
using DYFramework.Application;
using DYFramework.Domain;
using System.Linq;

namespace AuthenticationServer.Application
{
    public class UserInfoService : Service<UserInfoDataObject, UserInfo>, IUserInfoService
    {
        public UserInfoService(IRepository<UserInfo> repository, IMapper mapper) : base(repository, mapper)
        {

        }

        public UserInfoDataObject Get(string userName, string password)
        {
            var user = this.repository.Get(p =>
              (p.Email == userName || p.UserName == userName) && p.Password == password).FirstOrDefault();
            return this.mapper.Map<UserInfo, UserInfoDataObject>(user);
        }
    }
}
