using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(IDataProviderFactory providerFactory, INavigationService navigationService)
        {
            ProviderFactory = providerFactory;
            NavigationService = navigationService;
        }

        public IDataProviderFactory ProviderFactory { get; }
        public INavigationService NavigationService { get; }

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
                    NavigationService.Navigate<CustomersViewModel>(new CustomersViewState { OrderByDesc = r => r.CreatedOn });
                    break;
                case "Orders":
                    NavigationService.Navigate<OrdersViewModel>(new OrdersViewState { OrderByDesc = r => r.OrderDate });
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
            var request = new PageRequest<Customer>
            {
                PageSize = 5,
                OrderByDesc = r => r.CreatedOn
            };
            var page = await dataProvider.GetCustomersAsync(request);
            Customers = page.Items;
        }

        private async Task LoadProductsAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<Product>
            {
                PageSize = 5,
                OrderByDesc = r => r.CreatedOn
            };
            var page = await dataProvider.GetProductsAsync(request);
            Products = page.Items;
        }

        private async Task LoadOrdersAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<Order>
            {
                PageSize = 5,
                OrderByDesc = r => r.OrderDate
            };
            var page = await dataProvider.GetOrdersAsync(request);
            Orders = page.Items;
        }
    }
}
