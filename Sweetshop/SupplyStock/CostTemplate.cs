using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SupplyStock.Utils;

namespace SupplyStock
{
    public class CostTemplate : INotifyPropertyChanged, ICloneable
    {
        #region Variables

        private Currency _sellingPrice;

        #endregion
        #region Contructors

        public CostTemplate(string name, string description = "", int producedUnits = 0)
        {
            _sellingPrice = new Currency(0, CurrencyUnit.CUC);
            UnderlyingSupply = new Supply(name, description, producedUnits, MeasurementUnit.Unit, Cost.Amount,
                                          Cost.Unit) {FormatAmount = {Unit = MeasurementUnit.Unit}, IsTemplate = true};
        }
        public CostTemplate()
        {
            _sellingPrice = new Currency();
        }

        #endregion
        #region Properties

        public int CostTemplateId { get; set; }
        public string Name
        {
            get
            {
                if (UnderlyingSupply == null) UnderlyingSupply = new Supply();
                return UnderlyingSupply.Name;
            } 
            set
            {
                UnderlyingSupply.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get
            {
                if (UnderlyingSupply == null) UnderlyingSupply = new Supply();
                return UnderlyingSupply.Description;
            } 
            set
            {
                UnderlyingSupply.Description = value;
                OnPropertyChanged("Description");
            }
        }
        public Currency SellingPrice
        {
            get { return _sellingPrice; } 
            set
            {
                //if (this.FinishedTemplate)
                //{
                    _sellingPrice.Amount = value.Amount;
                    _sellingPrice.Unit = value.Unit;
                //}
            }
        }
        public double ProducedUnits
        {
            get
            {
                if (UnderlyingSupply == null) UnderlyingSupply = new Supply();
                if (UnderlyingSupply.FormatAmount == null) UnderlyingSupply.FormatAmount = new Measurement();
                UnderlyingSupply.FormatAmount.Unit = MeasurementUnit.Unit;
                return UnderlyingSupply.FormatAmount.Amount;
            } 
            set
            {
                UnderlyingSupply.FormatAmount.Amount = value;
                UnderlyingSupply.FormatAmount.Unit = MeasurementUnit.Unit;
                OnPropertyChanged("ProducedUnits");
            }
        }
        public bool FinishedTemplate { get; set; }
        public virtual List<SupplyAmount> SupplyAmounts { get; set; }

        public virtual Supply UnderlyingSupply { get; set; }

        //Not Mapped
        public string Image { get { return UnderlyingSupply == null ? null : UnderlyingSupply.Image; } set { UnderlyingSupply.Image = value; } }
        public Currency Cost
        {
            get
            {
                if (UnderlyingSupply == null) UnderlyingSupply = new Supply(){IsTemplate = true};
                UnderlyingSupply.FormatCost = new Currency(SuppliesCost().Sum(c => c.AmountCUC), CurrencyUnit.CUC);
                return UnderlyingSupply.FormatCost;
            }
            set { throw new ArgumentException("You can't modify the cost of a template explicity"); }
        }
        public Currency CostPerUnit { get { return new Currency((Cost.Amount / ProducedUnits), Cost.Unit); } }

        public Currency Profit
        {
            get
            {
                return FinishedTemplate 
                    ? new Currency(Math.Max(SellingPrice.AmountCUC * ProducedUnits - Cost.AmountCUC, 0), CurrencyUnit.CUC) 
                    : new Currency(0, CurrencyUnit.CUC);
            }
        }

        public string Percentage
        {
            get
            {
                return ((Profit * 100) / Cost).Amount.SmartString();
            }
        }
        public Currency ProfitPerUnit { get { return Profit/ProducedUnits; } }

        #endregion
        #region Methods


        public IEnumerable<Currency> SuppliesCost()
        {
            if (SupplyAmounts == null) return new List<Currency>();
            return SupplyAmounts.Select(sa => sa.Cost);
        }
        public bool Compare(CostTemplate b)
        {
            return Name == b.Name &&
                   (Description == "" || b.Description == "" || Description == b.Description);
        }
        public override string ToString()
        {
            return String.Format("{0} ({1} u-{2})", Name, ProducedUnits, Cost);
        }

        public object Clone()
        {
            var copy = new CostTemplate(Name, Description, (int)ProducedUnits)
                               {
                                   CostTemplateId = CostTemplateId,
                                   FinishedTemplate = FinishedTemplate,
                                   SellingPrice = {Amount = SellingPrice.Amount, Unit = SellingPrice.Unit},
                                   SupplyAmounts = SupplyAmounts,
                                   Image = Image
                               };
            return copy;
        }
        public void Copy(CostTemplate template)
        {
            Name = template.Name;
            Description = template.Description;
            ProducedUnits = template.ProducedUnits;
            FinishedTemplate = template.FinishedTemplate;            
            SellingPrice.Amount = template.SellingPrice.Amount;
            SellingPrice.Unit = template.SellingPrice.Unit;
            SupplyAmounts = template.SupplyAmounts;
            Image = template.Image;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void ItemChange(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
