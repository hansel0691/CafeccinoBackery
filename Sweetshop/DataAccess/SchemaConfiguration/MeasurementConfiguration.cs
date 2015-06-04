using System.Data.Entity.ModelConfiguration;
using SupplyStock.Utils;

namespace DataAccess.SchemaConfiguration
{
    public class MeasurementConfiguration : EntityTypeConfiguration<Measurement>
    {
        public MeasurementConfiguration()
        {
            Property(m => m.Amount).IsRequired();
            HasKey(m => m.Id);
            Ignore(m => m.AmountInUnit);
            Ignore(m => m.AllUnits);
            Ignore(m => m.RelatedUnits);

            HasOptional(m => m.Supply).WithOptionalDependent(s => s.FormatAmount);
        }
    }
}
