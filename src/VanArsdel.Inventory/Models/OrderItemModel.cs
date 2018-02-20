using System;
using System.Threading.Tasks;

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

            Product = new ProductModel(source.Product);
        }

        public long OrderID { get; set; }
        public int OrderLine { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int TaxType { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;
        public decimal Total => Subtotal * (1 + DataHelper.GetTaxRate(TaxType) / 100m);

        public ProductModel Product { get; private set; }

        public async Task LoadAsync()
        {
            await Product.LoadAsync();
        }
    }
}
