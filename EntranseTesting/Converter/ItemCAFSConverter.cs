using Avalonia.Data.Converters;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Converter
{
    public class ItemCAFSConverter: IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            return values;
        }
    }
}
