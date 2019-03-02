using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.Repositories.TypeConfigurations
{
    public class RelationRFIDQRCodeTypeConfiguration:TypeConfiguration<RelationRFIDQRCode>
    {
        public override void Configure(EntityTypeBuilder<RelationRFIDQRCode> builder)
        {
            
            base.Configure(builder);
        }
    }
}
