using System;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class OrderItemModel : ModelBase
    {
        public OrderItemModel()
        {
        }
        public OrderItemModel(OrderItem source)
        {
            OrderID = source.OrderID;
            OrderLine = source.OrderLine;
            ProductID = source.ProductID;
            Quantity = source.Quantity;
            UnitPrice = source.UnitPrice;
            Discount = source.Discount;
            TaxType = source.TaxType;
        }

        public long OrderID { get; set; }
        public int OrderLine { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int TaxType { get; set; }
    }
}
