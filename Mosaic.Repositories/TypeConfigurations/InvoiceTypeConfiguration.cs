using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.TypeConfigurations
{
    public class InvoiceTypeConfiguration:TypeConfiguration<Invoice>
    {
        public override void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.Ignore(p => p.GroupNoArray);
            base.Configure(builder);
        }
    }
}
