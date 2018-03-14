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

        public bool IsNew => OrderLine <= 0;

        public override void Merge(ModelBase source)
        {
            if (source is OrderItemModel model)
            {
                Merge(model);
            }
        }

        public void Merge(OrderItemModel source)
        {
            if (source != null)
            {
                OrderID = source.OrderID;
                OrderLine = source.OrderLine;
                ProductID = source.ProductID;
                Quantity = source.Quantity;
                UnitPrice = source.UnitPrice;
                Discount = source.Discount;
                TaxType = source.TaxType;
                Product = source.Product;
            }
        }
    }
}
