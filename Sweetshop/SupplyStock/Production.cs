using System.Collections.Generic;
using System.Linq;
using SupplyStock.Utils;

namespace SupplyStock
{
    public class Production
    {
        #region Contructors

        public Production()
        {
        }
        public Production(IEnumerable<TemplateAmount> templateAmounts = null)
        {
            TemplateAmounts = new List<TemplateAmount>(templateAmounts);
        }

        #endregion
        #region Properties

        public int ProductionId { get; set; }
        public string Name { get; set; }
        public virtual List<TemplateAmount> TemplateAmounts { get; set; }
        public string Description { get; set; }
        public Currency Cost { get { return TemplateAmounts == null ? new Currency() : new Currency(TemplateAmounts.Sum(ta => ta.Cost.ToCUC().Amount), CurrencyUnit.CUC); } }
        public List<SupplyAmount> SupplyAmounts
        {
            get
            {
                if (TemplateAmounts == null) TemplateAmounts = new List<TemplateAmount>();
                var usedSupplies = new List<SupplyAmount>();
                foreach (var ta in TemplateAmounts)
                {
                    foreach (var sa in ta.ImpliedSupplies)
                    {
                        var index = usedSupplies.FindIndex(supp => supp.Supply.ToString() == sa.Supply.ToString()); 
                        if (index != -1)
                            usedSupplies[index].StartAmount += sa.Amount;
                        else
                            usedSupplies.AddRange(sa.UsedSupplies()); //***********
                    }
                }
                return usedSupplies;
            }
        }
        public Currency Profits
        {
            get
            {
                return new Currency(TemplateAmounts == null ? new double() : TemplateAmounts.Sum(ta => ta.Profits.ToCUC().Amount), CurrencyUnit.CUC);
            }
        }

        #endregion
        #region Methods

        public void Copy(Production production)
        {
            Name = production.Name;
            Description = production.Description;
        }
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
