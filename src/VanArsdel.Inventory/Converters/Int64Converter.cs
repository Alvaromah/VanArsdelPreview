using System;

using Windows.UI.Xaml.Data;

namespace VanArsdel.Inventory.Converters
{
    public sealed class Int64Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Int64 n64)
            {
                return n64 == 0 ? "" : n64.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (Int64.TryParse(value.ToString(), out Int64 n64))
                {
                    return n64;
                }
            }
            return (Int64)0;
        }
    }
}
