using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.TypeConfigurations
{
    public class QRCodeTypeConfiguration:TypeConfiguration<QRCode>
    {
        public override void Configure(EntityTypeBuilder<QRCode> builder)
        {
            
            base.Configure(builder);
        }
    }
}
