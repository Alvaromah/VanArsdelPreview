using System;
using System.Collections.Generic;

namespace VanArsdel.Inventory.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public IEnumerable<NavigationItem> Items => GetNavigationItems();

        private object _selectedItem = null;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { Set(ref _selectedItem, value); }
        }

        private IEnumerable<NavigationItem> GetNavigationItems()
        {
            yield return KnownNavigationItems.Dashboard;
            yield return KnownNavigationItems.Customers;
            yield return KnownNavigationItems.Orders;
            yield return KnownNavigationItems.Products;
        }
    }
}
