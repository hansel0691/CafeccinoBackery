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
    public class SupplyVM : INotifyPropertyChanged
    {
        #region Variables

        private readonly MContext _context;
        
        private string _name;
        private string _description;

        #endregion
        #region Contructors

        public SupplyVM(MContext context, Supply supply = null)
        {
            _context = context;
            Cost = new Currency();
            Amount = new Measurement();
            
            if (supply == null)
            {
                Supply = new Supply(){FormatAmount = new Measurement(), FormatCost = new Currency()};
                return;
            }
            Supply = supply;
            ResetToSupply(true);
            Cost = (Currency)Supply.FormatCost.Clone();
            Amount = (Measurement)Supply.FormatAmount.Clone();

            RelatedSupplies.Sort((s1, s2) => (int)Math.Sign(s1.CostPerUnit().AmountCUC - s2.CostPerUnit().AmountCUC));
            var relatedtemplates = new List<CostTemplate>(_context.LocalTemplates.Where(t => t.SupplyAmounts != null && t.SupplyAmounts.Any(sa => sa.Supply.ToString() == Supply.ToString())));
            RelatedTemplateSupplies = new ObservableCollection<TemplateSupplyVM>(relatedtemplates.Select(t => new TemplateSupplyVM(t, Supply)));
        }

        #endregion
        #region Properties

        public Supply Supply { get; set; }

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
        public Currency Cost{ get; set; }

        public double CostAmount
        {
            get
            {
                return Cost.Amount;
            }
            set
            {
                if (Cost.Amount != value)
                {
                    Cost.Amount = value;
                    OnPropertyChanged("Cost");
                }
            }
        }
        public CurrencyUnit CostUnit
        {
            get
            {
                return Cost.Unit;
            }
            set
            {
                if (Cost.Unit != value)
                {
                    Cost.Unit = value;
                    OnPropertyChanged("Cost");
                }
            }
        }
        public Measurement Amount { get; set; }
        public double MeasurementAmount
        {
            get { return Amount.Amount; }
            set
            {
                if (Amount.Amount != value)
                {
                    Amount.Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }
        public MeasurementUnit MeasurementUnit
        {
            get { return Amount.Unit; }
            set
            {
                if (Amount.Unit != value)
                {
                    Amount.Unit = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        public string Image { get; set; }
        public bool IsTemplate { get; private set; }

        public ObservableCollection<TemplateSupplyVM> RelatedTemplateSupplies { get; set; }
        public List<Supply> RelatedSupplies { get
        {
            var result =
                _context.LocalSupplies.Where(s => s.Name == _name && s.ToString() != Supply.ToString())
                    .ToList();
            result.Sort((s1, s2) => s1.CostPerUnit().Amount.CompareTo(s2.CostPerUnit().Amount));
            return result;
        } }
        public IEnumerable<string> PosibleUnits()
        {
            return Enumerable.Any(_context.SupplyAmounts,
                supplyAmount => supplyAmount.Supply.ToString() == Supply.ToString())
                ? Supply.FormatAmount.RelatedUnits
                : Supply.FormatAmount.AllUnits;
        }

        public IEnumerable<string> SuppliesName { get { return _context.LocalSupplies.Select(s => s.Name).Distinct(); } }

        #endregion
        #region Methods

        public void ResetImage()
        {
            var realSupply = _context.GetSupply(Supply);
            realSupply.Image = null;
            _context.SaveChanges();
        }
        public IEnumerable<TemplateSupplyVM> FilterTemplates(string text)
        {
            return text != "Filtrar Fichas de Costo..." ? RelatedTemplateSupplies.Where(s => s.Template.Name.ToLower().Contains(text.ToLower())) : RelatedTemplateSupplies;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void ConvertFormatCost(string s)
        {
            var newCost = s == "CUC" ? Cost.ToCUC() : Cost.ToCUP();
            CostAmount = newCost.Amount;
            CostUnit = newCost.Unit;
        }
        public void ConvertFormatAmount(string s)
        {
            var newAmount =  Amount.ConvertTo(Measurement.ToMeasurementUnit(s));
            MeasurementAmount = newAmount.Amount;
            MeasurementUnit = newAmount.Unit;
        }
        public void RecalculateTemplateCosts(Supply supply)
        {
            RelatedTemplateSupplies.ToList().ForEach(temp => temp.Supply = supply);
        }
        public void ResetTemplateCosts()
        {
            RelatedTemplateSupplies.ToList().ForEach(temp => temp.Supply = Supply);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Sort(string header, ListSortDirection direction)
        {
            Comparison<TemplateSupplyVM> comparer = null;

            switch (header)
            {
                case "Nombre":
                    comparer = (x, y) => x.Template.Name.CompareTo(y.Template.Name);
                    break;
                case "Descripción":
                    comparer = (x, y) => x.Template.Description == null ? 1 : y.Template.Description == null ? -1 : x.Template.Description.CompareTo(y.Template.Description);
                    break;
                case "Cantidad Utilizada":
                    comparer = (x, y) => x.UsedAmount.AmountInUnit.CompareTo(y.UsedAmount.AmountInUnit);
                    break;
                case "Costo":
                    comparer = (x, y) => x.Cost.ToCUC().CompareTo(y.Cost.ToCUC());
                    break;
                case "Costo Total":
                    comparer = (x, y) => x.TotalCost.ToCUC().CompareTo(y.TotalCost.ToCUC());
                    break;
            }

            if (comparer == null) return;
            var items = new List<TemplateSupplyVM>(RelatedTemplateSupplies);
            RelatedTemplateSupplies.Clear();
            items.Sort(comparer);
            if (direction == ListSortDirection.Descending)
                items.Reverse();
            items.ForEach(s => RelatedTemplateSupplies.Add(s));

        }

        #endregion

        public void SaveSupply()
        {
            Supply.Copy(new Supply(_name, _description, Amount.Amount, Amount.Unit,
                Cost.Amount, Cost.Unit) {Image = Image, IsTemplate = IsTemplate});
            if (Supply.SupplyId == 0)
                _context.AddSupply(Supply);
            else
                _context.UpdateSupply(Supply);
        }

        public void ResetToSupply(bool soft = false)
        {
            Name = Supply.Name;
            Description = Supply.Description;
            Image = Supply.Image;
            IsTemplate = Supply.IsTemplate;
            if (!soft)
            {
                CostAmount = Supply.FormatCost.Amount;
                CostUnit = Supply.FormatCost.Unit;
                MeasurementAmount = Supply.FormatAmount.Amount;
                MeasurementUnit = Supply.FormatAmount.Unit;
            }
        }
    }
}
