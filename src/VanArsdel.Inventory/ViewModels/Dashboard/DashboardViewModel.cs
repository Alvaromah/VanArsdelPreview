using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

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
            Customers = page.Items;
        }

        private async Task LoadProductsAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<Product>
            {
                PageSize = 5,
                OrderBy = (r) => r.CreatedOn,
                Descending = true
            };
            var page = await dataProvider.GetProductsAsync(request);
            Products = page.Items;
        }

        private async Task LoadOrdersAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<Order>
            {
                PageSize = 5,
                OrderBy = (r) => r.OrderDate,
                Descending = true
            };
            var page = await dataProvider.GetOrdersAsync(request);
            Orders = page.Items;
        }
    }
}
