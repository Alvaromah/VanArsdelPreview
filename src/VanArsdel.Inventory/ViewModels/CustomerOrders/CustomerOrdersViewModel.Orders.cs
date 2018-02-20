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
        public string OrderQuery { get; set; }
        public string OrderQuotedQuery => String.IsNullOrEmpty(OrderQuery) ? " " : $"results for \"{OrderQuery}\"";

        private IList<OrderModel> _orders;
        public IList<OrderModel> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }

        private IList<OrderItemModel> _orderItems = null;
        public IList<OrderItemModel> OrderItems
        {
            get => _orderItems;
            set => Set(ref _orderItems, value);
        }

        private OrderModel _selectedOrder = null;
        public OrderModel SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (Set(ref _selectedOrder, value))
                {
                    RefreshOrderItems();
                }
            }
        }

        private OrderItemModel _selectedOrderItem = null;
        public OrderItemModel SelectedOrderItem
        {
            get => _selectedOrderItem;
            set => Set(ref _selectedOrderItem, value);
        }

        private int _ordersCount;
        public int OrdersCount
        {
            get => _ordersCount;
            set => Set(ref _ordersCount, value);
        }

        public int OrderItemsCount => OrderItems?.Count ?? 0;

        private int _orderPageIndex = 0;
        public int OrderPageIndex
        {
            get => _orderPageIndex;
            set { if (Set(ref _orderPageIndex, value)) RefreshOrders(); }
        }

        public bool IsOrderDataAvailable => (_orders?.Count ?? 0) > 0;
        public bool IsOrderDataUnavailable => !IsOrderDataAvailable;

        public async void RefreshOrders(bool resetPageIndex = false)
        {
            if (resetPageIndex)
            {
                _orderPageIndex = 0;
            }
            await RefreshOrdersAsync();
        }

        private async Task RefreshOrdersAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await RefreshOrdersAsync(dataProvider);
            }
        }

        private async Task RefreshOrdersAsync(IDataProvider dataProvider)
        {
            Orders = null;
            SelectedOrder = null;
            RaiseUpdateBindings();

            if (SelectedCustomer != null)
            {
                var page = await dataProvider.GetOrdersAsync(OrderPageIndex, 10, OrderQuery, SelectedCustomer.CustomerID);
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
                _orderPageIndex = page.PageIndex;
                RaiseUpdateBindings();
            }
        }

        private async void RefreshOrderItems()
        {
            OrderItems = null;
            SelectedOrderItem = null;
            RaiseUpdateBindings();

            if (SelectedOrder != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    var items = await dataProvider.GetOrderItemsAsync(SelectedOrder.OrderID);
                    var models = items.Select(r => new OrderItemModel(r)).ToList();
                    foreach (var model in models)
                    {
                        await model.LoadAsync();
                    }
                    OrderItems = models;
                    SelectedOrderItem = OrderItems.FirstOrDefault();
                    RaiseUpdateBindings();
                }
            }
        }

        public async Task DeleteCurrentOrderAsync()
        {
            var order = SelectedOrder;
            if (order != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    await dataProvider.DeleteOrder(order.OrderID);
                    await RefreshOrdersAsync(dataProvider);
                }
            }
        }
    }
}
