using System.Data.Entity.ModelConfiguration;
using SupplyStock.Utils;

namespace DataAccess.SchemaConfiguration
{
    class SupplyAmountConfiguration : EntityTypeConfiguration<SupplyAmount>
    {
        public SupplyAmountConfiguration()
        {
            Ignore(t => t.Amount);
            Ignore(t => t.Cost);
            Ignore(t => t.PosibleUnits);
            //Ignore(t => t.StartAmount);
            //Ignore(t => t.EndAmount);
            //Ignore(t => t.NonDefaultMeasuremnt);

            HasRequired(sa => sa.Supply).WithMany(s => s.SupplyAmounts).WillCascadeOnDelete(true);
            
        }
    }
}
