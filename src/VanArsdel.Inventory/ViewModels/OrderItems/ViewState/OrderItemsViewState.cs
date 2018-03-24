using System;
using System.Linq.Expressions;

using VanArsdel.Data;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemsViewState : ListViewState
    {
        static public OrderItemsViewState CreateDefault() => new OrderItemsViewState();

        public OrderItemsViewState()
        {
            OrderBy = r => r.OrderLine;
        }

        public long OrderID { get; set; }

        public Expression<Func<OrderItem, object>> OrderBy { get; set; }
        public Expression<Func<OrderItem, object>> OrderByDesc { get; set; }
    }
}
