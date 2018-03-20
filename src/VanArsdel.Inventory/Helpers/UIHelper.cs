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

        public string TotalItems(int count)
        {
            return $"Total {count} items";
        }
    }
}
