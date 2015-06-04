using System;
using System.Globalization;
using System.Windows.Data;

namespace BackeryApp.ClassUtils
{
    public class DefinedConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textValue = value as string;
            double doubleValue;
            if (!double.TryParse(textValue, out doubleValue))
                return 0;
            return doubleValue < 0 ? 0 : doubleValue;

        }

        #endregion
    }
}
