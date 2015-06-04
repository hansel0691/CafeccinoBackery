using System;
using System.Globalization;
using System.Windows.Data;
using SupplyStock.Utils;

namespace BackeryApp.ClassUtils
{
    class MeasurementConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var unit = (MeasurementUnit)value;
            return Measurement.UnitToString(unit);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var unit = value as string;
            if (unit == null) return null;
                //throw new ArgumentException("value must be string.");
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

        #endregion
    }
}
