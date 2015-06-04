using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DataAccess;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp.ViewModel
{
    public class CostTemplateVM : INotifyPropertyChanged
    {
        #region Variables

        private readonly MContext _context;
        private readonly Currency _cost;
        private readonly Currency _costPerUnit;
        private readonly Currency _price;
        private readonly Currency _profits;

	    #endregion
        #region Contructor

        public CostTemplateVM(MContext context, CostTemplate template)
        {
            _context = context;
            Template = template;
            _cost = template.Cost;
            _costPerUnit= template.CostPerUnit;
            _price = template.SellingPrice;
            _profits = template.Profit;
            UsedSupplies = new ObservableCollection<SupplyAmountVM>(template.SupplyAmounts.Select(sa => new SupplyAmountVM(sa)));
        }

        #endregion
        #region Properties

        public CostTemplate Template { get; set; }
        public Currency TemplateCost
        {
            get { return _cost; }
            set
            {
                _cost.Amount = value.Amount;
                _cost.Unit = value.Unit;
                OnPropertyChanged();
            }
        }
        public Currency TemplateCostPerUnit
        {
            get { return _costPerUnit; }
            set
            {
                _costPerUnit.Amount = value.Amount;
                _costPerUnit.Unit = value.Unit;
                OnPropertyChanged();
            }
        }
        public Currency TemplatePrice
        {
            get { return _price; }
            set
            {
                _price.Amount = value.Amount;
                _price .Unit= value.Unit;
                OnPropertyChanged();
            }
        }
        public Currency TemplateProfits
        {
            get { return _profits; }
            set
            {
                _profits.Amount = value.Amount;
                _profits.Unit = value.Unit;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SupplyAmountVM> UsedSupplies { get; private set; }
        public List<String> AllUnits { get { return new List<string> { "CUC", "CUP" }; } }

        #endregion
        #region Methods

        public void ResetImage()
        {
            var realTemplate = _context.GetTemplate(Template);
            realTemplate.Image = null;
            _context.SaveChanges();
        }
        public void ConvertCost(string s)
        {
            var convertedProfit = ConvertCurrency(TemplateCost, s);
            TemplateCost = convertedProfit;

            foreach (var sa in UsedSupplies)
            {
                var newCost = ConvertCurrency(sa.Cost, s);
                sa.Cost = newCost;
            }
        }
        public void ConvertCostPerUnit(string s)
        {
            var convertedProfit = ConvertCurrency(TemplateCostPerUnit, s);
            TemplateCostPerUnit = convertedProfit;
        }
        public void ConvertSellingPrice(string s)
        {
            var convertedProfit = ConvertCurrency(TemplatePrice, s);
            TemplatePrice = convertedProfit;
        }
        public void ConvertProfits(string s)
        {
            var convertedProfit = ConvertCurrency(TemplateProfits, s);
            TemplateProfits = convertedProfit;
        }
        private Currency ConvertCurrency(Currency amount, string unit)
        {
            return unit == "CUC" ? amount.ToCUC() : amount.ToCUP();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Sort(string header, ListSortDirection direction)
        {
            Comparison<SupplyAmount> comparer = null;

            switch (header)
            {
                case "Nombre":
                    comparer = (x, y) => x.Supply.ToString().CompareTo(y.Supply.ToString());
                    break;
                case "Descripción":
                    comparer = (x, y) => x.Supply.Description == null ? 1 : y.Supply.Description == null ? -1 : x.Supply.Description.CompareTo(y.Supply.Description);
                    break;
                case "Cantidad Utilizada":
                    comparer = (x, y) => x.Amount.AmountInUnit.CompareTo(y.Amount.AmountInUnit);
                    break;
                case "Costo":
                    comparer = (x, y) => x.Cost.ToCUC().CompareTo(y.Cost.ToCUC());
                    break;
            }

            if (comparer == null) return;
            var items = new List<SupplyAmountVM>(UsedSupplies);
            UsedSupplies.Clear();
            items.Sort((Comparison<SupplyAmountVM>) comparer);
            if (direction == ListSortDirection.Descending)
                items.Reverse();
            items.ForEach(s => UsedSupplies.Add(s));
        }

        #endregion
        
    }
}
