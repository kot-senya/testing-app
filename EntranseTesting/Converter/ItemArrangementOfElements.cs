using Avalonia.Data.Converters;
using EntranseTesting.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Converter
{
    public class ItemArrangementOfElements : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (int)((decimal?)parameter * ((decimal?)value - (decimal?)(14.0) + 100)/100);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (int)((decimal?)parameter * ((decimal?)value - (decimal?)(14.0) + 100) / 100);
        }
    }

}
