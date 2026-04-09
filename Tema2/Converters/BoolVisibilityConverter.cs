using System;
using System.Collections.Generic;
using System.Media;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace Tema2.Converters
{
    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool b = (bool)value;
                if (b)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
                throw new ArgumentException();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
