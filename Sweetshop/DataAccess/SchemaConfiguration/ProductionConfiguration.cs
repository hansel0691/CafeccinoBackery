using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupplyStock;

namespace DataAccess.SchemaConfiguration
{
    class ProductionConfiguration : EntityTypeConfiguration<Production>
    {
        public ProductionConfiguration()
        {
            Ignore(t => t.Cost);
            Ignore(t => t.Profits);
            Ignore(t => t.SupplyAmounts);
            Property(t => t.Name).IsRequired();

            HasMany(t => t.TemplateAmounts).WithRequired();
        }
    }
}
