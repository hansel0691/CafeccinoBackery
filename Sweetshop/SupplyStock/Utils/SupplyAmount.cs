using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SupplyStock.Utils
{
    public class SupplyAmount : INotifyPropertyChanged
    {
        #region Variables

        private bool _nonDefaultProperty;

        #endregion
        #region Contructor

        public SupplyAmount(Supply supply, Measurement startAmount, Measurement endAmount, bool nonDefaultMeasurement)
        {
            Supply = supply;
            StartAmount = startAmount;
            EndAmount = endAmount;
            NonDefaultMeasuremnt = nonDefaultMeasurement;
        }
        public SupplyAmount(Supply supply, Measurement measurement)
        {
            Supply = supply;
            StartAmount = measurement;
            EndAmount = new Measurement();
            NonDefaultMeasuremnt = false;
        }
        public SupplyAmount()
        {
        }

        #endregion
        #region Properties

        public int SupplyAmountId { get; set; }
        public virtual Supply Supply { get; set; }
        public virtual Measurement StartAmount { get; set; }
        public virtual Measurement EndAmount { get; set; }
        public bool NonDefaultMeasuremnt
        {
            get { return _nonDefaultProperty; }
            set
            {
                if (_nonDefaultProperty == value) return;
                _nonDefaultProperty = value;
                OnPropertyChanged("NonDefaultMeasuremnt");
            }
        }

        public virtual CostTemplate Template { get; set; }

        //Not mapped
        public virtual Measurement Amount
        {
            get
            {
                return StartAmount -
                       (EndAmount != null && NonDefaultMeasuremnt
                            ? EndAmount
                            : new Measurement(0, StartAmount.Unit));
            }
        }
        public Currency Cost
        {
            get
            {
                var costPerUnit = Supply.CostPerUnit();
                return new Currency(costPerUnit.Amount * Amount.AmountInUnit, costPerUnit.Unit);
            }
        }
        public IEnumerable<string> PosibleUnits { get { return Supply.FormatAmount.GetRelatedUnits().Select(Measurement.UnitToString); } }

        public IEnumerable<string> ExtendedPosibleUnits
        {
            get
            {
                var relatedUnit = Supply.FormatAmount.GetRelatedUnits().Select(Measurement.UnitToString);
                return Supply.FormatAmount.Unit == MeasurementUnit.Unit
                    ? relatedUnit
                    : new[] {"u"}.Concat(relatedUnit);
            } 
        }

        #endregion
        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void PropertyChange(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        public IEnumerable<SupplyAmount> UsedSupplies()
        {
            if (!this.Supply.IsTemplate)
            {
                yield return this;
                yield break;
            }
            var temp = this.Supply.Template;
            var result = new List<SupplyAmount>();
            foreach (var supplyAmount in temp.SupplyAmounts)
                result.AddRange(supplyAmount.UsedSupplies().Select(sa => new SupplyAmount(sa.Supply, sa.Amount*this.Amount.Amount)));

            foreach (var supplyAmount in result)
                yield return supplyAmount;
        }
    }
}
