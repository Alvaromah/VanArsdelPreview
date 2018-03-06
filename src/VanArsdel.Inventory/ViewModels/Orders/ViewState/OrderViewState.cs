using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderViewState
    {
        static public OrderViewState CreateDefault() => new OrderViewState();

        public long OrderID { get; set; }

        public bool IsNew => OrderID <= 0;
    }
}
