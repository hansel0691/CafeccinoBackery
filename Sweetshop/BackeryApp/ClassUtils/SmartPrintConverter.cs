using System;
using System.Globalization;
using System.Windows.Data;
using SupplyStock.Utils;

namespace BackeryApp.ClassUtils
{
    public class SmartPrintConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = (double)value;
            return doubleValue.SmartString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
