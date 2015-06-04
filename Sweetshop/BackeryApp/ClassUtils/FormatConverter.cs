using System;
using System.Globalization;
using System.Windows.Data;

namespace BackeryApp.ClassUtils
{
    class FormatConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (stringValue == null) return "error";
            return stringValue.Substring(stringValue.LastIndexOf('('));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
