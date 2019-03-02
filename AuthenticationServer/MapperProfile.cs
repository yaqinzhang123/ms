using AuthenticationServer.DataObjects;
using AuthenticationServer.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            //Entity TO Dto
            CreateMap<UserInfo, UserInfoDataObject>()
                .ForMember(dest => dest.AppID, src => src.MapFrom(p => p.AppInfo.AppId))
                .ForMember(dest => dest.RoleIDList, src => src.MapFrom(p => p.UserRoleList.Select(k => k.Role.ID)));
            CreateMap<AppInfo, AppInfoDataObject>()
                .ForMember(dest => dest.UserIDList, src => src.MapFrom(p => p.UserList.Select(k => k.ID)));
            CreateMap<Manager, ManagerDataObject>()
                .ForMember(dest => dest.AppIDList, src => src.MapFrom(p => p.AppList.Select(k => k.ID)));
            CreateMap<Role, RoleDataObject>();


            //Dto TO Entity

            CreateMap<ManagerDataObject, Manager>();
            CreateMap<AppInfoDataObject, AppInfo>();
            CreateMap<RoleDataObject, Role>();
            CreateMap<UserInfoDataObject, UserInfo>();

        }
    }
}
