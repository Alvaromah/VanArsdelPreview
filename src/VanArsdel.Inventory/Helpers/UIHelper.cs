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

        public string ToShortDate(DateTime? date)
        {
            return (date?.ToShortDateString()) ?? "";
        }

        public string TotalItems(int count)
        {
            return $"Total {count} items";
        }
    }
}
