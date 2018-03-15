﻿using System;

namespace VanArsdel.Inventory.Models
{
    public class OrderItemModel : ModelBase
    {
        public long OrderID { get; set; }
        public int OrderLine { get; set; }

        public string ProductID { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { if (Set(ref _quantity, value)) UpdateTotals(); }
        }

        private int _taxType;
        public int TaxType
        {
            get => _taxType;
            set { if (Set(ref _taxType, value)) UpdateTotals(); }
        }

        private decimal _discount;
        public decimal Discount
        {
            get => _discount;
            set { if (Set(ref _discount, value)) UpdateTotals(); }
        }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;

        public decimal Total => (Subtotal - Discount) * (1 + DataHelper.GetTaxRate(TaxType) / 100m);

        public ProductModel Product { get; set; }

        public bool IsNew => OrderLine <= 0;

        private void UpdateTotals()
        {
            NotifyPropertyChanged(nameof(Subtotal));
            NotifyPropertyChanged(nameof(Total));
        }

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
