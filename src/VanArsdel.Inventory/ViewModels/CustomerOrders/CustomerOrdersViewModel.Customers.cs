using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.ViewModels
{
    partial class CustomerOrdersViewModel
    {
        public string CustomerQuery { get; set; }
        public string CustomerQuotedQuery => String.IsNullOrEmpty(CustomerQuery) ? " " : $"results for \"{CustomerQuery}\"";

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
            set
            {
                if (Set(ref _selectedCustomer, value))
                {
                    OrderQuery = null;
                    _orderPageIndex = 0;
                    RefreshOrders();
                }
            }
        }

        private int _customersCount;
        public int CustomersCount
        {
            get => _customersCount;
            set => Set(ref _customersCount, value);
        }

        private int _customerPageIndex = 0;
        public int CustomerPageIndex
        {
            get => _customerPageIndex;
            set { if (Set(ref _customerPageIndex, value)) RefreshCustomers(); }
        }

        public bool IsCustomerDataAvailable => (_customers?.Count ?? 0) > 0;
        public bool IsCustomerDataUnavailable => !IsCustomerDataAvailable;

        public async void RefreshCustomers(bool resetPageIndex = false)
        {
            if (resetPageIndex)
            {
                _customerPageIndex = 0;
            }
            await RefreshCustomersAsync();
        }

        private async Task RefreshCustomersAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await RefreshCustomersAsync(dataProvider);
            }
        }

        private async Task RefreshCustomersAsync(IDataProvider dataProvider)
        {
            Customers = null;
            SelectedCustomer = null;
            RaiseUpdateBindings();

            var page = await dataProvider.GetCustomersAsync(CustomerPageIndex, 25, CustomerQuery);
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
            _customerPageIndex = page.PageIndex;
            RaiseUpdateBindings();
        }
    }
}
