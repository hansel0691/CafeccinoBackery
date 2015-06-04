using System.ComponentModel;
using System.Runtime.CompilerServices;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp.ViewModel
{
    public class SupplyAmountVM : INotifyPropertyChanged
    {
        #region Variables

        private readonly Currency _cost;
        private readonly Measurement _measurement;
        private int _multiplier;

        #endregion
        #region Contructor

        public SupplyAmountVM(SupplyAmount supplyAmount)
        {
            Supply = (Supply)supplyAmount.Supply.Clone();
            _measurement = supplyAmount.Amount;
            _cost = supplyAmount.Cost;
            _multiplier = 1;
        }

        #endregion
        #region Properties

        public Supply Supply { get; set; }

        public Measurement Measurement
        {
            get { return _measurement * Multiplier; }
            set
            {
                _measurement.Amount = value.Amount/Multiplier;
                _measurement.Unit = value.Unit;
                OnPropertyChanged();
            }
        }

        public Currency Cost
        {
            get { return _cost * Multiplier; }
            set
            {
                _cost.Amount = value.Amount/Multiplier;
                _cost.Unit = value.Unit;
                OnPropertyChanged();
            }
        }

        public int Multiplier
        {
            get
            {
                return _multiplier;
            }
            set
            {
                _multiplier = value;
                OnPropertyChanged("Cost");
                OnPropertyChanged("Measurement");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
