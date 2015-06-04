using System;
using System.Collections.Generic;
using SupplyStock.Utils;

namespace SupplyStock
{
    public class Supply : ICloneable
    {
        #region Constructors

        public Supply(string name, string description, double amount, MeasurementUnit unit, double cost, CurrencyUnit cunit)
        {
            Name = name;
            Description = description;
            FormatAmount = new Measurement(amount, unit);
            FormatCost = new Currency(cost, cunit);
        }
        public Supply()
        {
            FormatCost = new Currency();
        }

        #endregion
        #region Properties

        public int SupplyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Measurement FormatAmount { get; set; }
        public Currency FormatCost { get; set; }
        public bool IsTemplate { get; set; }
        public string Image { get; set; }
        
        public virtual CostTemplate Template { get; set; }
        public virtual List<SupplyAmount> SupplyAmounts { get; set; }

        #endregion
        #region Methods

        public Currency CostPerUnit()
        {
            return Math.Abs(FormatAmount.Amount - 0) > 0.000001 && Math.Abs(FormatCost.Amount - 0) > 0.000001
                       ? new Currency(FormatCost.Amount/(double) FormatAmount.AmountInUnit,
                                      FormatCost.Unit)
                       : new Currency();
        }
        public bool Equals(Supply b)
        {
            return b != null && Name == b.Name && FormatAmount.Equals(b.FormatAmount) &&
                   FormatCost.Equals(b.FormatCost) &&
                   (Description == "" || b.Description == "" || Description == b.Description);
        }
        public override string ToString()
        {
            if (FormatAmount == null) FormatAmount = new Measurement();
            return String.Format("{0} ({1}-{2})", Name, FormatAmount.ToString(), FormatCost.ToString());
        }

        public object Clone()
        {
            return new Supply(Name, Description, FormatAmount.Amount, FormatAmount.Unit,
                                    FormatCost.Amount, FormatCost.Unit) { SupplyId = SupplyId, IsTemplate = IsTemplate, Image = Image };
        }
        public void Copy(Supply supply)
        {
            //this.SupplyId = supply.SupplyId;
            Name = supply.Name;
            Description = supply.Description;
            FormatAmount.Amount = supply.FormatAmount.Amount;
            FormatAmount.Unit = supply.FormatAmount.Unit;
            FormatCost.Amount = supply.FormatCost.Amount;
            FormatCost.Unit = supply.FormatCost.Unit;
            //this.IsTemplate = supply.IsTemplate;
            Image = supply.Image;

        }

        #endregion
    }
}
