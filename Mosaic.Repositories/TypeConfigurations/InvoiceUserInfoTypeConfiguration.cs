using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.TypeConfigurations
{
    public class InvoiceUserInfoTypeConfiguration:TypeConfiguration<InvoiceUserInfo>
    {
        public override void Configure(EntityTypeBuilder<InvoiceUserInfo> builder)
        {
            builder.Ignore(p => p.GroupNoArray);
            base.Configure(builder);
        }
    }
}
