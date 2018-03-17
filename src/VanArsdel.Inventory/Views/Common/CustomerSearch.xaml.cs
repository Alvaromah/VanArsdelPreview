using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;
using VanArsdel.Data;
using System.Windows.Input;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomerSearch : UserControl
    {
        public CustomerSearch()
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

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList<CustomerModel>), typeof(CustomerSearch), new PropertyMetadata(null));
        #endregion

        #region CustomerSelectedCommand
        public ICommand CustomerSelectedCommand
        {
            get { return (ICommand)GetValue(CustomerSelectedCommandProperty); }
            set { SetValue(CustomerSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty CustomerSelectedCommandProperty = DependencyProperty.Register(nameof(CustomerSelectedCommand), typeof(ICommand), typeof(CustomerSearch), new PropertyMetadata(null));
        #endregion

        private async void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (args.CheckCurrent())
                {
                    Items = await GetItems(sender.Text);
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

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            CustomerSelectedCommand?.TryExecute(args.ChosenSuggestion);
        }
    }
}
