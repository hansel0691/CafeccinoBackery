using System.ComponentModel;
using System.Linq;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp.ViewModel
{
    public class TemplateSupplyVM : INotifyPropertyChanged
    {
        #region Varibales

        private Supply _supply;

        #endregion
        #region Constructor

        public TemplateSupplyVM(CostTemplate template, Supply supply)
        {
            Template = template;
            _supply = supply;

            Cost = template.SupplyAmounts.First(sa => sa.Supply.Name == Supply.Name).Cost;
            TotalCost = template.Cost;
            UsedAmount = Template.SupplyAmounts.First(sa => sa.Supply.ToString() == Supply.ToString()).Amount;
        }

        #endregion
        #region Properties

        public CostTemplate Template { get; set; }

        public Supply Supply
        {
            get
            {
                return _supply;
            }
            set
            {
                var diff = value.CostPerUnit() - Supply.CostPerUnit();
                Cost += diff*UsedAmount.AmountInUnit;
                TotalCost += diff * UsedAmount.AmountInUnit;
                _supply = value;
                OnPropertyChanged("Cost");
                OnPropertyChanged("TotalCost");
            }
        }

        public Measurement UsedAmount { get; set; }
        public Currency Cost { get; set; }
        public Currency TotalCost { get; set; }

        #endregion
        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseEvent()
        {
            OnPropertyChanged("Cost");
            OnPropertyChanged("TotalCost");
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
