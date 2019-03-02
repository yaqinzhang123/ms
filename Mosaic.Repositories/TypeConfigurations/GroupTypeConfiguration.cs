using DYFramework.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaic.Domain.Models;


namespace Mosaic.Repositories.TypeConfigurations
{
    public class GroupTypeConfiguration:TypeConfiguration<Group>
    {
        public override void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Ignore(p => p.No);
            base.Configure(builder);
        }
    }
}
