﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.Controls
{
    public sealed partial class CustomerSuggestBox : UserControl
    {
        public CustomerSuggestBox()
        {
            ProviderFactory = new DataProviderFactory();
            InitializeComponent();
        }

        private IDataProviderFactory ProviderFactory { get; }

        #region Items
        public IList<CustomerModel> Items
        {
            get { return (IList<CustomerModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList<CustomerModel>), typeof(CustomerSuggestBox), new PropertyMetadata(null));
        #endregion

        #region DisplayText
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(nameof(DisplayText), typeof(string), typeof(CustomerSuggestBox), new PropertyMetadata(null));
        #endregion

        #region IsReadOnly*
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        private static void IsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomerSuggestBox;
            control.suggestBox.Mode = ((bool)e.NewValue == true) ? TextEditMode.ReadOnly : TextEditMode.Auto;
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(CustomerSuggestBox), new PropertyMetadata(false, IsReadOnlyChanged));
        #endregion

        #region CustomerSelectedCommand
        public ICommand CustomerSelectedCommand
        {
            get { return (ICommand)GetValue(CustomerSelectedCommandProperty); }
            set { SetValue(CustomerSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty CustomerSelectedCommandProperty = DependencyProperty.Register(nameof(CustomerSelectedCommand), typeof(ICommand), typeof(CustomerSuggestBox), new PropertyMetadata(null));
        #endregion

        private async void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (args.CheckCurrent())
                {
                    Items = String.IsNullOrEmpty(sender.Text) ? null : await GetItems(sender.Text);
                }
            }
        }

        private async Task<IList<CustomerModel>> GetItems(string query)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                var request = new PageRequest<Customer>(0, 20)
                {
                    Query = query,
                    OrderBy = r => r.FirstName
                };
                var page = await dataProvider.GetCustomersAsync(request);
                return page.Items;
            }
        }

        private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            CustomerSelectedCommand?.TryExecute(args.SelectedItem);
        }
    }
}
