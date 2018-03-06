using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomerViewState
    {
        static public CustomerViewState CreateDefault() => new CustomerViewState();

        public long CustomerID { get; set; }

        public bool IsNew => CustomerID <= 0;
    }
}
