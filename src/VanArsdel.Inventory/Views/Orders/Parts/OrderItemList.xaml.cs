using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrderItemList : UserControl
    {
        public OrderItemList()
        {
            InitializeComponent();
        }

        #region ItemsSource
        public IList<OrderItemModel> ItemsSource
        {
            get { return (IList<OrderItemModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OrderItemList;
            control.Bindings.Update();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<OrderItemModel>), typeof(OrderItemList), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region SelectedItem
        public OrderItemModel SelectedItem
        {
            get { return (OrderItemModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(OrderItemModel), typeof(OrderItemList), new PropertyMetadata(null));
        #endregion

        public bool IsDataAvailable => (ItemsSource?.Count ?? 0) > 0;
        public bool IsDataUnavailable => !IsDataAvailable;

        public string DataUnavailableMessage => ItemsSource == null ? "Loading..." : "No items found";
    }
}
