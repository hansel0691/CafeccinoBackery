using System;
using System.Globalization;
using System.Windows.Data;

namespace BackeryApp.ClassUtils
{
    class AmountConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            double doubleValue;
            if (stringValue != null && !String.IsNullOrWhiteSpace(stringValue) && double.TryParse(stringValue, out doubleValue))
                return doubleValue;
            return 0;
        }

        #endregion
    }
}
