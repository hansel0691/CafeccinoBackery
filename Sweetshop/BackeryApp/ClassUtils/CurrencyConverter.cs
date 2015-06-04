using System;
using System.Globalization;
using System.Windows.Data;
using SupplyStock.Utils;

namespace BackeryApp.ClassUtils
{
    public class CurrencyConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Currency) return null;
            var unit = (CurrencyUnit)value;
            return unit == CurrencyUnit.CUC ? 0 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var unit = (int)value;
            return unit == 0 ? CurrencyUnit.CUC : CurrencyUnit.CUP;
        }

        #endregion
    }
}
