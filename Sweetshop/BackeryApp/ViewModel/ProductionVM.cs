using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using DataAccess;
using SupplyStock;
using SupplyStock.Utils;
using MessageBox = System.Windows.MessageBox;

namespace BackeryApp.ViewModel
{
    public class ProductionVM : INotifyPropertyChanged
    {
        #region Variables

        private readonly MContext _context;
        private string _name;
        private string _description;
        private readonly Currency _cost;
        private readonly Currency _selectedCost;
        private readonly Currency _profits;

        #endregion
        #region Constructor

        public ProductionVM(MContext context, Production production)
        {
            Production = production;
            if (production == null) Production = new Production(new List<TemplateAmount>());
            
            _name = Production.Name;
            _description = Production.Description;
            _cost = (Currency)Production.Cost.Clone();
            _profits = (Currency)Production.Profits.Clone();
            _selectedCost = new Currency();

            _context = context;
            SearchPattern = "";
            Templates = new ObservableCollection<CostTemplate>();
            Filter();
            TemplateAmounts = new ObservableCollection<TemplateAmount>(Production.TemplateAmounts.Select(ta => new TemplateAmount(ta.Template, ta.Amount)));
            SupplyAmounts = new List<SupplyAmountVM>(Production.SupplyAmounts.Select(sa => new SupplyAmountVM(sa)));
            Days = 1;
        }

        #endregion
        #region Properties

        public Production Production { get; private set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public Currency Cost
        {
            get { return _cost*Days; }
            set
            {
                _cost.Amount = value.Amount/Days;
                _cost.Unit = value.Unit;
                OnPropertyChanged();
            }
        }
        public Currency Profits
        {
            get { return _profits * Days; }
            set
            {
                _profits.Amount = value.Amount/Days;
                _profits.Unit = value.Unit;
                OnPropertyChanged();
            }
        }
        public List<SupplyAmountVM> SupplyAmounts { get; set; }

        public ObservableCollection<CostTemplate> Templates { get; private set; }
        public ObservableCollection<TemplateAmount> TemplateAmounts { get; set; }

        public Currency SelectedCost
        {
            get { return _selectedCost; }
            set
            {
                _selectedCost.Amount = value.Amount;
                _selectedCost.Unit = value.Unit;
                OnPropertyChanged();
            }
        }

        public string SearchPattern { get; set; }
        public int Days { get; set; }

        public string[] AllUnits { get { return new[] {"CUC", "CUP"}; } }

        #endregion
        #region Methods

        public void Filter()
        {
            Templates.Clear();
            foreach (var temp in _context.LocalTemplates.Where(t => t.FinishedTemplate && PassTheFilter(t)))
                Templates.Add(temp);
        }

        public bool PassTheFilter(CostTemplate template)
        {
            return template.Name.ToLower().Contains(SearchPattern.ToLower());
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SaveProduction()
        {
            if (String.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Debe asignar un nombre a la producción antes de crearla.", "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            Production.Copy(new Production(new List<TemplateAmount>()){Name = Name, Description = Description});
            if (Production.ProductionId == 0)
            {
                Production.TemplateAmounts = new List<TemplateAmount>(TemplateAmounts);
                _context.AddProduction(Production);
                //_context.AddTemplateToProduction(Production, TemplateAmounts);
            }
            else
            {
                var toRemove = new List<int>();
                Production.TemplateAmounts.Where(ta => !TemplateAmounts.Contains(ta)).ToList().ForEach(ta => toRemove.Add(ta.TemplateAmountId));
                toRemove.ForEach(id => _context.RemoveTemplateAmount(id));
                Production.TemplateAmounts.AddRange(TemplateAmounts.Where(ta => !Production.TemplateAmounts.Contains(ta)));
                _context.UpdateProduction();
                ResetToProduction(true);
            }
            //ResetToProduction();
        }

        public void ClearUsedTemplates()
        {
            TemplateAmounts.Clear();
            Cost = new Currency(0, Cost.Unit);
            SelectedCost = new Currency();
        }

        public void AddTemplate(CostTemplate template)
        {
            if (TemplateAmounts.Any(ta => ta.Template.ToString() == template.ToString())) return;
            TemplateAmounts.Add(new TemplateAmount(template, 0));
        }

        public void ResetToProduction(bool deep = false)
        {
            Name = Production.Name;
            Description = Production.Description;
            if (deep)
            {
                SupplyAmounts.Clear();
                Production.SupplyAmounts.ForEach(sa => SupplyAmounts.Add(new SupplyAmountVM(sa)));
                TemplateAmounts.Clear();
                Production.TemplateAmounts.ForEach(ta => TemplateAmounts.Add(new TemplateAmount(ta.Template, ta.Amount)));
            }
            Cost = Production.Cost;
            Profits = Production.Profits;
        }

        public void RecalculateTotalCost()
        {
            Cost = new Currency(0, Cost.Unit);
            foreach (var ta in TemplateAmounts)
                Cost += ta.Cost;
        }

        public void DaysChange()
        {
            OnPropertyChanged("Cost");
            OnPropertyChanged("Profits");
            SupplyAmounts.ForEach(sa => sa.Multiplier = Days);
        }
    }
}
