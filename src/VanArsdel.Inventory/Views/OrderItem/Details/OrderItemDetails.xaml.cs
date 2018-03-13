using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrderItemDetails : UserControl
    {
        public OrderItemDetails()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderItemDetailsViewModel ViewModel
        {
            get { return (OrderItemDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(OrderItemDetailsViewModel), typeof(OrderItemDetails), new PropertyMetadata(null));
        #endregion

        public void SetFocus()
        {
            details.SetFocus();
        }
    }
}
