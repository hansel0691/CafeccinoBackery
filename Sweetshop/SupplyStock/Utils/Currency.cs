using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyStock.Utils
{
    public enum CurrencyUnit
    {
        CUC,
        CUP
    }

    [ComplexType]
    public class Currency : INotifyPropertyChanged, ICloneable
    {
        #region Variables

        private double _amount;
        private CurrencyUnit _unit;

        #endregion
        #region Constructor

        public Currency(double amount, CurrencyUnit unit, int rate = 24)
        {
            Amount = amount;
            Unit = unit;
        }
        public Currency()
        {
        }

        #endregion
        #region Properties

        public double Amount
        {
            get { return _amount; } 
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("Identifier");
                }
            }
        }
        public CurrencyUnit Unit
        {
            get { return _unit; } 
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged("Unit");
                    OnPropertyChanged("Identifier");
                }
            }
        }

        [NotMapped]
        public List<String> AllUnits { get{return new List<string>{"CUC", "CUP"};} }
        [NotMapped]
        public double AmountCUC { get { return ToCUC().Amount; } }
        [NotMapped]
        public string Identifier { get { return ToString(); } }

        #endregion
        #region Methods

        public Currency ToCUC()
        {
            return Unit == CurrencyUnit.CUC ? this : new Currency(Amount / Settings.Options.CurrencyRatio, CurrencyUnit.CUC);
        }
        public Currency ToCUP()
        {
            return Unit == CurrencyUnit.CUP ? this : new Currency(Amount * Settings.Options.CurrencyRatio, CurrencyUnit.CUP);
        }
        public static Currency operator /(Currency a, Currency b)
        {
            var bCopy = a.Unit == CurrencyUnit.CUC ? b.ToCUC() : b.ToCUP();
            return new Currency(a.Amount/b.Amount, a.Unit);
        }
        public static Currency operator /(Currency a, double b)
        {
            return new Currency(a.Amount / b, a.Unit);
        }
        public static Currency operator *(Currency a, double b)
        {
            return new Currency(a.Amount * b, a.Unit);
        }
        public static Currency operator -(Currency a, Currency b)
        {
            var diff = a.AmountCUC - b.AmountCUC;
            return a.Unit == CurrencyUnit.CUC ? new Currency(diff, CurrencyUnit.CUC) : new Currency(diff, CurrencyUnit.CUC).ToCUP();
        }
        public static Currency operator +(Currency a, Currency b)
        {
            return a - (b*-1);
        }
        public bool Equals(Currency b)
        {
            return b != null && (Math.Abs(AmountCUC - b.AmountCUC) < 0.0001);
        }
        public override string ToString()
        {
            return string.Format("{0} {1}", Amount.SmartString(), Unit);
        }

        public object Clone()
        {
            return new Currency(Amount, Unit);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(Currency currency)
        {
            var c1 = ToCUC();
            var c2 = currency.ToCUC();
            return Math.Sign(c1.Amount - c2.Amount);
        }
    }
}
