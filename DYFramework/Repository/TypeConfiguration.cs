using DYFramework.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DYFramework.Repositories
{
    public class TypeConfiguration<TAggregateRoot> : IEntityTypeConfiguration<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        public virtual void Configure(EntityTypeBuilder<TAggregateRoot> builder)
        {
            builder.HasKey(p => p.ID);
            builder.ToTable(typeof(TAggregateRoot).Name);
            builder.Property(p => p.ID).UseSqlServerIdentityColumn();
        }
    }
}
