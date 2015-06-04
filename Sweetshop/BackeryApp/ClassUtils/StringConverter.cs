using System;
using System.Globalization;
using System.Windows.Data;

namespace BackeryApp.ClassUtils
{
    class StringConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            return String.IsNullOrWhiteSpace(text) ? "(sin descripción)" : text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
