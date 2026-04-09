using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Tema2.Converters
{
    internal class BoolVisibilityMultiConverter : IMultiValueConverter
    {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                foreach(var b in values)
                    if (!(bool)b)
                        return Visibility.Collapsed;
                return Visibility.Visible;
            }

            public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
