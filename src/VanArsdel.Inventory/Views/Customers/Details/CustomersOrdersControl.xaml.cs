using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersOrdersControl : UserControl
    {
        public CustomersOrdersControl()
        {
            InitializeComponent();
        }

        #region ItemsSource
        public IList<OrderModel> ItemsSource
        {
            get { return (IList<OrderModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomersOrdersControl;
            control.Bindings.Update();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<OrderModel>), typeof(CustomersOrdersControl), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region SelectedItem
        public OrderModel SelectedItem
        {
            get { return (OrderModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(OrderModel), typeof(CustomersOrdersControl), new PropertyMetadata(null));
        #endregion

        public bool IsDataEmpty => (ItemsSource?.Count ?? 0) == 0;
    }
}
