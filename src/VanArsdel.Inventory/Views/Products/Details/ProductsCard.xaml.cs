using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class ProductsCard : UserControl
    {
        public ProductsCard()
        {
            InitializeComponent();
        }

        #region Item
        public ProductModel Item
        {
            get { return (ProductModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(ProductModel), typeof(ProductsCard), new PropertyMetadata(null));
        #endregion
    }
}
