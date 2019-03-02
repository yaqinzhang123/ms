using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.TypeConfigurations
{
    public class CarDataTypeConfiguration:TypeConfiguration<CarData>
    {
        public override void Configure(EntityTypeBuilder<CarData> builder)
        {
            base.Configure(builder);
            builder.Ignore(p => p.Duration);
            builder.Property(p => p.Handled).HasDefaultValue(false);            
        }
    }
}
