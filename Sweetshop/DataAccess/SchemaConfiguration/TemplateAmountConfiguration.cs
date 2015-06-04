using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupplyStock;
using SupplyStock.Utils;

namespace DataAccess.SchemaConfiguration
{
    class TemplateAmountConfiguration : EntityTypeConfiguration<TemplateAmount>
    {
        public TemplateAmountConfiguration()
        {
            Ignore(ta => ta.Cost);
            Ignore(ta => ta.Profits);
            Ignore(ta => ta.ImpliedSupplies);
            HasRequired(ta => ta.Template).WithMany();
        }
    }
}
