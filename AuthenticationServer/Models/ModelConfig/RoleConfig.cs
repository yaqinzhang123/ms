using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models.ModelConfig
{
    public class RoleConfig:TypeConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasMany(p => p.UserRoleList).WithOne(p => p.Role);
            base.Configure(builder);
        }
    }
}
