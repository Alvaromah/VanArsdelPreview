using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public event EventHandler UpdateBindings;

        public OrdersViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public string Query { get; set; }

        public string QuotedQuery => String.IsNullOrEmpty(Query) ? " " : $"results for \"{Query}\"";

        private IList<OrderModel> _orders;
        public IList<OrderModel> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }

        private OrderModel _selectedOrder;
        public OrderModel SelectedOrder
        {
            get => _selectedOrder;
            set => Set(ref _selectedOrder, value);
        }

        private int _ordersCount;
        public int OrdersCount
        {
            get => _ordersCount;
            set => Set(ref _ordersCount, value);
        }

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => _pageIndex;
            set { if (Set(ref _pageIndex, value)) Refresh(); }
        }

        public bool IsDataAvailable => (_orders?.Count ?? 0) > 0;
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
            Orders = null;
            SelectedOrder = null;
            RaiseUpdateBindings();

            var page = await dataProvider.GetOrdersAsync(PageIndex, 25, Query);
            var models = page.Items.Select(r => new OrderModel(r)).ToList();
            foreach (var model in models)
            {
                await model.LoadAsync();
            }

            // First, update Order list
            Orders = models;

            // Then, update selected Order
            SelectedOrder = Orders.FirstOrDefault();

            // Finally update other properties, preventing firing Refresh() again
            _ordersCount = page.Count;
            _pageIndex = page.PageIndex;
            RaiseUpdateBindings();
        }

        public async Task DeleteCurrentAsync()
        {
            var order = SelectedOrder;
            if (order != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    await dataProvider.DeleteOrder(order.OrderID);
                    await RefreshAsync(dataProvider);
                }
            }
        }

        private void RaiseUpdateBindings() => UpdateBindings?.Invoke(this, EventArgs.Empty);
    }
}
