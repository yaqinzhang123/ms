using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.ModelConfig
{
    public class UserInfoConfig:TypeConfiguration<UserInfo>
    {
        public override void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasMany(p => p.UserRoleList).WithOne(p => p.UserInfo);
            base.Configure(builder);
        }
    }
}
