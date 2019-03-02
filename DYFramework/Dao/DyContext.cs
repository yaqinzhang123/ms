using DYFramework.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;


namespace DYFramework.Dao
{
    public abstract class DyContext:DbContext
    {
        public DyContext(DbContextOptions options):base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = Assembly.GetEntryAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.GetTypeInfo().BaseType != null && type.GetTypeInfo().BaseType == typeof(AggregateRoot));

            foreach (var type in entityTypes)
            {
                modelBuilder.Model.GetOrAddEntityType(type);
            }
            this.AddTypeConfiguration(modelBuilder);
            base.OnModelCreating(modelBuilder);

        }

        protected abstract void AddTypeConfiguration(ModelBuilder modelBuilder);
    }
}
