using System;

namespace VanArsdel.Inventory
{
    public class UIHelper
    {
        static public UIHelper Current { get; }

        static UIHelper()
        {
            Current = new UIHelper();
        }

        public bool NOT(bool value)
        {
            return !value;
        }

        public string ToShortDate(DateTimeOffset? date)
        {
            return (date?.Date.ToLocalTime().ToShortDateString()) ?? "";
        }

        public string ToLongDate(DateTimeOffset? date)
        {
            return (date?.Date.ToLocalTime().ToLongDateString()) ?? "";
        }

        public string TotalItems(int count)
        {
            return $"Total {count} items";
        }
    }
}
