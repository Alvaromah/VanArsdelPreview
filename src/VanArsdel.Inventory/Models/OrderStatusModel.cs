using System;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class OrderStatusModel : ModelBase
    {
        public OrderStatusModel()
        {
        }
        public OrderStatusModel(OrderStatus source)
        {
            Status = source.Status;
            Name = source.Name;
        }

        public int Status { get; set; }
        public string Name { get; set; }
    }
}
