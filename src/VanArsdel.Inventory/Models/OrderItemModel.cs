using System;

namespace VanArsdel.Inventory.Models
{
    public class OrderItemModel : ModelBase
    {
        public long OrderID { get; set; }
        public int OrderLine { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int TaxType { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;
        public decimal Total => Subtotal * (1 + DataHelper.GetTaxRate(TaxType) / 100m);

        public ProductModel Product { get; set; }
    }
}
