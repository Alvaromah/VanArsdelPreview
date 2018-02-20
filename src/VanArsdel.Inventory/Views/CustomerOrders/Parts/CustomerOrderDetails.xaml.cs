using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomerOrderDetails : UserControl
    {
        public CustomerOrderDetails()
        {
            InitializeComponent();
        }

        #region Model
        public OrderModel Model
        {
            get { return (OrderModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(OrderModel), typeof(CustomerOrderDetails), new PropertyMetadata(null));
        #endregion
    }
}
