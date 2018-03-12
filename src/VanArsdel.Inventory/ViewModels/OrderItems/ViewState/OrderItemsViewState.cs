using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemsViewState : ListViewState
    {
        static public OrderItemsViewState CreateDefault() => new OrderItemsViewState();

        public long OrderID { get; set; }
    }
}
