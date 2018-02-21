using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Controls;

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
            set
            {
                Set(ref _selectedCustomer, value);
                IsEditMode = false;
            }
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

        private DetailToolbarMode _toolbarMode = DetailToolbarMode.Default;
        public DetailToolbarMode ToolbarMode
        {
            get => _toolbarMode;
            set => Set(ref _toolbarMode, value);
        }

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (Set(ref _isEditMode, value))
                {
                    ToolbarMode = _isEditMode ? DetailToolbarMode.CancelSave : DetailToolbarMode.Default;
                }
            }
        }

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

            var page = await dataProvider.GetCustomersAsync(PageIndex, 10, Query);
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

        public void BeginEdit()
        {
            IsEditMode = true;
        }

        public void CancelEdit()
        {
            IsEditMode = false;
            SelectedCustomer?.UndoChanges();
            SelectedCustomer?.NotifyChanges();
        }

        public async Task SaveCurrentAsync()
        {
            IsEditMode = false;
            var customer = SelectedCustomer;
            if (customer != null)
            {
                customer.CommitChanges();
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    await dataProvider.UpdateCustomer(customer.Source);
                    customer.NotifyChanges();
                }
            }
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
