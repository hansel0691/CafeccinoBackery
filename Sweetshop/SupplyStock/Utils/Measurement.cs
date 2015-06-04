using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SupplyStock.Utils
{
    public enum MeasurementUnit { Kilogram, Gram, Pound, GallonUK, GallonUS, Milliliter, Liter, Unit }
    public enum MeasurementKind { Volume, Weight, Unit }

    public class Measurement : INotifyPropertyChanged, ICloneable
    {
        #region Variables

        private double _amount;
        private MeasurementUnit _unit;

        #endregion
        #region Constructors

        public Measurement(double amount, MeasurementUnit unit)
        {
            Amount = amount;
            Unit = unit;
        }
        public Measurement()
        {
        }

        #endregion
        #region Properties

        public int Id { get; set; }
        public double Amount
        {
            get { return _amount; } 
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged("Identifier");
                }
            }
        }
        public MeasurementUnit Unit
        {
            get { return _unit; } 
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged("Identifier");
                }
            }
        }

        public Supply Supply { get; set; }
        public double AmountInUnit
        {
            get
            {
                var kind = GetKind();
                switch (kind)
                {
                    case MeasurementKind.Unit:
                        return Amount;
                    case MeasurementKind.Weight:
                        return ConvertTo(MeasurementUnit.Gram).Amount;
                    default:
                        return ConvertTo(MeasurementUnit.Milliliter).Amount;
                }
            }
        }
        public IEnumerable<string> RelatedUnits { get { return GetRelatedUnits().Select(UnitToString); } }
        public List<string> AllUnits
        {
            get { return new List<string> { "g", "Lb", "Kg", "ml", "L", "G (UK)", "G (US)", "u" }; }
        }
        public string Identifier { get { return ToString(); } }
        
        #endregion
        #region Methods

        private double UpScale(double amount, int index, List<double> scale)
        {
            return index >= scale.Count ? amount : amount*scale[index];
        }
        private double DownScale(double amount, int index, List<double> scale)
        {
            return index < 0 ? amount : amount/scale[index];
        }
        public Measurement ConvertTo(MeasurementUnit unit)
        {
            var relatedUnits = GetRelatedUnits();
            var to = relatedUnits.IndexOf(unit);
            if (to == -1) throw new Exception(string.Format("Is imposible convert {0} to {1}", Unit, unit));
            var from = relatedUnits.IndexOf(Unit);
            var amount = Amount;
            var scale = GetScale();
            if (to > from)
                for (int i = from; i < to; i++)
                    amount = UpScale(amount, i, scale);
            else if (to != from)
                for (int i = from-1; i >= to; i--)
                    amount = DownScale(amount, i, scale);
            return new Measurement(Math.Round(amount, 9), unit);
        }
        
        public bool Equals(Measurement b)
        {
            return b != null && 
                   Math.Abs(Amount - b.Amount) < 0.0001 && Unit == b.Unit;
        }
        public MeasurementKind GetKind()
        {
            if (Unit == MeasurementUnit.Gram || Unit == MeasurementUnit.Pound || Unit == MeasurementUnit.Kilogram)
                return MeasurementKind.Weight;
            return Unit == MeasurementUnit.Unit ? MeasurementKind.Unit : MeasurementKind.Volume;
        }
        public List<MeasurementUnit> GetRelatedUnits()
        {
            switch (GetKind())
            {
                case MeasurementKind.Unit: return new List<MeasurementUnit>{MeasurementUnit.Unit};
                case MeasurementKind.Volume: return new List<MeasurementUnit> { MeasurementUnit.Milliliter, MeasurementUnit.Liter, MeasurementUnit.GallonUK, MeasurementUnit.GallonUS };
                default: return new List<MeasurementUnit> { MeasurementUnit.Gram, MeasurementUnit.Pound, MeasurementUnit.Kilogram };
            }
        }
        public List<double> GetScale()
        {
            switch (GetKind())
            {
                case MeasurementKind.Unit: return new List<double>{1};
                case MeasurementKind.Weight: return new List<double> { 1 / (0.45359237*1000), 0.45359237 };
                default :
                    return new List<double> { 0.001, 1 / 4.54609, 1.200949925504855};
            }
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", Amount.SmartString(), UnitToString(Unit));
        }

        public object Clone()
        {
            return new Measurement(Amount, Unit);
        }

        public static string UnitToString(MeasurementUnit unit)
        {
            switch (unit)
            {
                case MeasurementUnit.Kilogram:
                    return "Kg";
                case MeasurementUnit.Gram:
                    return "g";
                case MeasurementUnit.Pound:
                    return "Lb";
                case MeasurementUnit.GallonUS:
                    return "G (US)";
                case MeasurementUnit.GallonUK:
                    return "G (UK)";
                case MeasurementUnit.Milliliter:
                    return "ml";
                case MeasurementUnit.Liter:
                    return "L";
                default:
                    return "u";

            }
        }
        public static MeasurementUnit ToMeasurementUnit(string unit)
        {
            switch (unit)
            {
                case "Kg":
                    return MeasurementUnit.Kilogram;
                case "g":
                    return MeasurementUnit.Gram;
                case "Lb":
                    return MeasurementUnit.Pound;
                case "G (US)":
                    return MeasurementUnit.GallonUS;
                case "G (UK)":
                    return MeasurementUnit.GallonUK;
                case "ml":
                    return MeasurementUnit.Milliliter;
                case "L":
                    return MeasurementUnit.Liter;
                default:
                    return MeasurementUnit.Unit;
            }
        }

        public static Measurement operator *(Measurement a, double b)
        {
            return new Measurement(a.Amount * b, a.Unit);
        }
        public static Measurement operator /(Measurement a, double b)
        {
            return new Measurement(a.Amount / b, a.Unit);
        }
        public static Measurement operator -(Measurement a, Measurement b)
        {
            if (a.GetKind() != b.GetKind()) throw new ArgumentException("Los tipos de los operandos deben estar relacionados.");
            var amount = a.AmountInUnit - b.AmountInUnit;
            var unit = a.GetRelatedUnits().FirstOrDefault();
            return new Measurement(amount, unit).ConvertTo(a.Unit);
        }
        public static Measurement operator +(Measurement a, Measurement b)
        {
            return a - (b * -1);
        }
        public static Measurement operator *(Measurement a, Measurement b)
        {
            if (a.GetKind() != b.GetKind()) throw new ArgumentException("Los tipos de los operandos deben estar relacionados.");
            var amount = a.AmountInUnit * b.AmountInUnit;
            var unit = a.GetRelatedUnits().FirstOrDefault();
            return new Measurement(amount, unit).ConvertTo(a.Unit);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
