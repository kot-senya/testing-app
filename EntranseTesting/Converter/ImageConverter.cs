using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Converter
{
    public class ImageConverter : IValueConverter
    {

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int num = (int)((decimal?)parameter * (decimal?)value / (decimal?)(14.0));
            return (num > 550)? 550: num ;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int num = (int)((decimal?)parameter * (decimal?)value / (decimal?)(14.0));
            return (num > 550) ? 550 : num;
        }
    }
}
