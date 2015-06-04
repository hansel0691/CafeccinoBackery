using SupplyStock.Utils;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BackeryApp.ClassUtils
{
    class DifferenceConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currencyValue = value as Currency;
            var textResult = "";
            if (currencyValue == null)throw new Exception("Unknown error.");
            textResult += "(";
            textResult += currencyValue.Amount > 0 ? "+" : "-";
            textResult += (Math.Abs(currencyValue.Amount)).SmartString() + ")";
            return textResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
