using System;
using System.Globalization;
using System.Windows.Data;

namespace Paritet.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var year = ((DateTime)value).ToShortDateString();

            return year;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
