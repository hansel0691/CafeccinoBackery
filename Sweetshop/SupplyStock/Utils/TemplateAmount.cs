using System.Collections.Generic;
using System.Linq;

namespace SupplyStock.Utils
{
    public class TemplateAmount
    {
        #region Contructor

        public TemplateAmount()
        {
            
        }
        public TemplateAmount(CostTemplate template, int units)
        {
            Template = template;
            Amount = units;
        }

        #endregion
        #region Properties

        public int TemplateAmountId { get; set; }
        public virtual CostTemplate Template { get; set; }
        public int Amount { get; set; }
        public Currency Cost
        {
            get 
            {
                return Template.CostPerUnit*Amount;
            }
        }
        public IEnumerable<SupplyAmount> ImpliedSupplies     
        { 
            get 
            {
                return Template.SupplyAmounts.ToList().Select(sa => new SupplyAmount(sa.Supply, sa.Amount/Template.ProducedUnits*Amount));
            } 
        }
        public Currency Profits {
            get 
            {
                return Template.ProfitPerUnit*Amount;
            }
        }

        #endregion
        #region Methods

        

        #endregion

    }
}
