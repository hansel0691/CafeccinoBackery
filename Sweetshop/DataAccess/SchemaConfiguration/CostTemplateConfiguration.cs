using System.Data.Entity.ModelConfiguration;
using SupplyStock;

namespace DataAccess.SchemaConfiguration
{
    public class CostTemplateConfiguration : EntityTypeConfiguration<CostTemplate>
    {
        public CostTemplateConfiguration()
        {
            Ignore(t => t.Cost); 
            Ignore(t => t.CostPerUnit);
            Ignore(t => t.Profit);
            Ignore(t => t.Name);
            Ignore(t => t.Description);
            Ignore(t => t.ProducedUnits);
            Ignore(t => t.Image);

            HasMany(t => t.SupplyAmounts).WithRequired(sa => sa.Template);
            HasRequired(t => t.UnderlyingSupply).WithOptional(s => s.Template);
            //HasRequired(t => t.UnderlyingSupply).WithOptional();
        }
    }
}
