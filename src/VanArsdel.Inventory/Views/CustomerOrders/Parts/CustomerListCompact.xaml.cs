using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomerListCompact : UserControl
    {
        public CustomerListCompact()
        {
            InitializeComponent();
        }

        #region ItemsSource
        public IList<CustomerModel> ItemsSource
        {
            get { return (IList<CustomerModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomerListCompact;
            control.Bindings.Update();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<CustomerModel>), typeof(CustomerListCompact), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region SelectedItem
        public CustomerModel SelectedItem
        {
            get { return (CustomerModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(CustomerModel), typeof(CustomerListCompact), new PropertyMetadata(null));
        #endregion

        public bool IsDataAvailable => (ItemsSource?.Count ?? 0) > 0;
        public bool IsDataUnavailable => !IsDataAvailable;

        public string DataUnavailableMessage => ItemsSource == null ? "Loading..." : "No items found";
    }
}
