using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrdersListControl : UserControl
    {
        public OrdersListControl()
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
            var control = d as OrdersListControl;
            control.Bindings.Update();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<OrderModel>), typeof(OrdersListControl), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region SelectedItem
        public OrderModel SelectedItem
        {
            get { return (OrderModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(OrderModel), typeof(OrdersListControl), new PropertyMetadata(null));
        #endregion

        public bool IsDataAvailable => (ItemsSource?.Count ?? 0) > 0;
        public bool IsDataUnavailable => !IsDataAvailable;

        public string DataUnavailableMessage => ItemsSource == null ? "Loading..." : "No items found";
    }
}
