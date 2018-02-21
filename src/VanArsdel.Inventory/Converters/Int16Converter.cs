using System;

using Windows.UI.Xaml.Data;

namespace VanArsdel.Inventory.Converters
{
    public class Int16Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Int16 n16)
            {
                return n16 == 0 ? "" : n16.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if (Int16.TryParse(value.ToString(), out Int16 n16))
                {
                    return n16;
                }
            }
            return (Int16)0;
        }
    }
}
