using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        private IList<CustomerModel> _customers = null;
        public IList<CustomerModel> Customers
        {
            get => _customers;
            set => Set(ref _customers, value);
        }

        private IList<ProductModel> _products = null;
        public IList<ProductModel> Products
        {
            get => _products;
            set => Set(ref _products, value);
        }

        private IList<OrderModel> _orders = null;
        public IList<OrderModel> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }

        public void ItemSelected(string item)
        {
            switch (item)
            {
                case "Customers":
                    NavigateTo(KnownNavigationItems.Customers);
                    break;
                case "Orders":
                    NavigateTo(KnownNavigationItems.Orders);
                    break;
                default:
                    break;
            }
        }

        public async Task LoadAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await LoadCustomersAsync(dataProvider);
                await LoadProductsAsync(dataProvider);
                await LoadOrdersAsync(dataProvider);
            }
        }

        public void Unload()
        {
            Customers = null;
            Products = null;
            Orders = null;
        }

        private async Task LoadCustomersAsync(IDataProvider dataProvider)
        {
            var page = await dataProvider.GetCustomersAsync(0, 5);
            var models = page.Items.Select(r => new CustomerModel(r)).ToList();
            foreach (var model in models)
            {
                await model.LoadAsync();
            }
            Customers = models;
        }

        private async Task LoadProductsAsync(IDataProvider dataProvider)
        {
            var page = await dataProvider.GetProductsAsync(0, 5);
            var models = page.Items.Select(r => new ProductModel(r)).ToList();
            foreach (var model in models)
            {
                await model.LoadAsync();
            }
            Products = models;
        }

        private async Task LoadOrdersAsync(IDataProvider dataProvider)
        {
            var page = await dataProvider.GetOrdersAsync(0, 5);
            var models = page.Items.Select(r => new OrderModel(r)).ToList();
            foreach (var model in models)
            {
                await model.LoadAsync();
            }
            Orders = models;
        }
    }
}
