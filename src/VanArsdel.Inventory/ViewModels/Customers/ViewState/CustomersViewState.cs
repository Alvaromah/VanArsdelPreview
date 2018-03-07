using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomersViewState : ListViewState
    {
        static public CustomersViewState CreateDefault() => new CustomersViewState
        {
            PageIndex = 0,
            PageSize = 20
        };

        private CustomersViewState()
        {
        }
    }
}
