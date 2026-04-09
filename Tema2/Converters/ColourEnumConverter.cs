using System;
using System.Collections.Generic;
using System.Media;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace Tema2.Converters
{
    public class ColourEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is char c)
            {
                if (c == 'l')
                    return Brushes.LightGray;
                else
                {
                    if (c == 'g')
                        return Brushes.Green;
                    else
                        return Brushes.Red;
                }
            }
            return Binding.DoNothing;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
