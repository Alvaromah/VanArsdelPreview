using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomersViewState : ListViewState
    {
        static public CustomersViewState CreateDefault() => new CustomersViewState();

        private CustomersViewState()
        {
        }
    }
}
