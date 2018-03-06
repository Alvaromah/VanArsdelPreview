using System;

using Windows.UI.Xaml.Data;

namespace VanArsdel.Inventory.Converters
{
    public sealed class Int32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Int32 n32)
            {
                return n32 == 0 ? "" : n32.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (Int32.TryParse(value.ToString(), out Int32 n32))
                {
                    return n32;
                }
            }
            return 0;
        }
    }
}
