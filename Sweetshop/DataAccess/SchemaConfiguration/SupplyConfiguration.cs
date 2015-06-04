using System.Data.Entity.ModelConfiguration;
using SupplyStock;

namespace DataAccess.SchemaConfiguration
{
    public class SupplyConfiguration : EntityTypeConfiguration<Supply>
    {
        public SupplyConfiguration()
        {
            Property(s => s.Name).IsRequired().HasMaxLength(100);
            Property(s => s.Description).HasMaxLength(500);

        }
    }
}
