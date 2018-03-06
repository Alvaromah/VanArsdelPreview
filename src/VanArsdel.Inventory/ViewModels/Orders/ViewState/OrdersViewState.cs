using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrdersViewState
    {
        static public OrdersViewState CreateDefault() => new OrdersViewState();

        public long CustomerID { get; set; }
    }
}
