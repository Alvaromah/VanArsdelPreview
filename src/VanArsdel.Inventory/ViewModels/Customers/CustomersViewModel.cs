using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public event EventHandler UpdateBindings;

        public CustomersViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public string Query { get; set; }

        public string QuotedQuery => String.IsNullOrEmpty(Query) ? " " : $"results for \"{Query}\"";

        private IList<CustomerModel> _customers;
        public IList<CustomerModel> Customers
        {
            get => _customers;
            set => Set(ref _customers, value);
        }

        private CustomerModel _selectedCustomer;
        public CustomerModel SelectedCustomer
        {
            get => _selectedCustomer;
            set => Set(ref _selectedCustomer, value);
        }

        private int _customersCount;
        public int CustomersCount
        {
            get => _customersCount;
            set => Set(ref _customersCount, value);
        }

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => _pageIndex;
            set { if (Set(ref _pageIndex, value)) Refresh(); }
        }

        public bool IsDataAvailable => (_customers?.Count ?? 0) > 0;
        public bool IsDataUnavailable => !IsDataAvailable;

        public async Task LoadAsync()
        {
            await RefreshAsync();
        }

        public async void Refresh(bool resetPageIndex = false)
        {
            if (resetPageIndex)
            {
                _pageIndex = 0;
            }
            await RefreshAsync();
        }
        private async Task RefreshAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await RefreshAsync(dataProvider);
            }
        }
        private async Task RefreshAsync(IDataProvider dataProvider)
        {
            Customers = null;
            SelectedCustomer = null;
            RaiseUpdateBindings();

            var page = await dataProvider.GetCustomersAsync(PageIndex, 25, Query);
            var models = page.Items.Select(r => new CustomerModel(r)).ToList();
            foreach (var model in models)
            {
                await model.LoadAsync();
            }

            // First, update Customer list
            Customers = models;

            // Then, update selected Customer
            SelectedCustomer = Customers.FirstOrDefault();

            // Finally update other properties, preventing firing Refresh() again
            _customersCount = page.Count;
            _pageIndex = page.PageIndex;
            RaiseUpdateBindings();
        }

        public async Task DeleteCurrentAsync()
        {
            var customer = SelectedCustomer;
            if (customer != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    await dataProvider.DeleteCustomer(customer.CustomerID);
                    await RefreshAsync(dataProvider);
                }
            }
        }

        private void RaiseUpdateBindings() => UpdateBindings?.Invoke(this, EventArgs.Empty);
    }
}
