using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemViewState
    {
        static public OrderItemViewState CreateDefault() => new OrderItemViewState();

        public long OrderID { get; set; }
        public int OrderLine { get; set; }

        public bool IsNew => OrderLine <= 0;
    }
}
