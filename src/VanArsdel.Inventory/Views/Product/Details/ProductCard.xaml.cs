using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class ProductCard : UserControl
    {
        public ProductCard()
        {
            InitializeComponent();
        }

        #region Item
        public ProductModel Item
        {
            get { return (ProductModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(ProductModel), typeof(ProductCard), new PropertyMetadata(null));
        #endregion
    }
}
