using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersListControl : UserControl
    {
        public CustomersListControl()
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
            var control = d as CustomersListControl;
            control.Bindings.Update();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<CustomerModel>), typeof(CustomersListControl), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region SelectedItem
        public CustomerModel SelectedItem
        {
            get { return (CustomerModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(CustomerModel), typeof(CustomersListControl), new PropertyMetadata(null));
        #endregion

        #region SelectedItems
        public ObservableCollection<CustomerModel> SelectedItems
        {
            get { return (ObservableCollection<CustomerModel>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<CustomerModel>), typeof(CustomersListControl), new PropertyMetadata(null));
        #endregion

        #region IsMultipleSelection
        public bool IsMultipleSelection
        {
            get { return (bool)GetValue(IsMultipleSelectionProperty); }
            set { SetValue(IsMultipleSelectionProperty, value); }
        }

        private static void IsMultipleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomersListControl;
            control.Bindings.Update();
        }

        public static readonly DependencyProperty IsMultipleSelectionProperty = DependencyProperty.Register("IsMultipleSelection", typeof(bool), typeof(CustomersListControl), new PropertyMetadata(false, IsMultipleSelectionChanged));
        #endregion

        public ListViewSelectionMode SelectionMode => IsMultipleSelection ? ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;

        public bool IsDataAvailable => (ItemsSource?.Count ?? 0) > 0;
        public bool IsDataUnavailable => !IsDataAvailable;

        public string DataUnavailableMessage => ItemsSource == null ? "Loading..." : "No items found";

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItems != null)
            {
                foreach (CustomerModel item in e.AddedItems)
                {
                    SelectedItems.Add(item);
                }
                foreach (CustomerModel item in e.RemovedItems)
                {
                    SelectedItems.Remove(item);
                }
            }
        }
    }
}
