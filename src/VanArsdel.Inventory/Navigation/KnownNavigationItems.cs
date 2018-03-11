using System;

using VanArsdel.Inventory.Views;

namespace VanArsdel.Inventory
{
    static public class KnownNavigationItems
    {
        static public readonly NavigationItem Dashboard = new NavigationItem(0xE80F, "Dashboard", typeof(DashboardView));
        static public readonly NavigationItem Customers = new NavigationItem(0xE716, "Customers", typeof(CustomersView));
        static public readonly NavigationItem Orders = new NavigationItem(0xE14C, "Orders", typeof(OrdersView));
        static public readonly NavigationItem Products = new NavigationItem(0xECAA, "Products", typeof(CommonView));

        static public readonly NavigationItem Settings = new NavigationItem(0, "Settings", typeof(SettingsView));
    }
}
