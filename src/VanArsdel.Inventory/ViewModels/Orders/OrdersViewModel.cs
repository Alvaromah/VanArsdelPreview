using System;
using System.ComponentModel;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public OrdersViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;
            MessageService = serviceManager.MessageService;

            OrderList = new OrderListViewModel(ProviderFactory, serviceManager);
            OrderDetails = new OrderDetailsViewModel(ProviderFactory, serviceManager);
            OrderItemList = new OrderItemListViewModel(ProviderFactory, serviceManager);
        }

        public IDataProviderFactory ProviderFactory { get; }
        public IMessageService MessageService { get; }

        public OrderListViewModel OrderList { get; set; }
        public OrderDetailsViewModel OrderDetails { get; set; }
        public OrderItemListViewModel OrderItemList { get; set; }

        public async Task LoadAsync(OrdersViewState state)
        {
            MessageService.Subscribe<OrderListViewModel>(this, OnMessage);
            await OrderList.LoadAsync(state);
        }

        public void Unload()
        {
            OrderList.Unload();
            MessageService.Unsubscribe(this);
        }

        public async Task RefreshAsync()
        {
            await OrderList.RefreshAsync();
        }

        public void CancelEdit()
        {
            OrderDetails.CancelEdit();
        }

        private async void OnMessage(object sender, string message, object args)
        {
            if (sender == OrderList && message == "ItemSelected")
            {
                await Dispatcher.RunIdleAsync((e) =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            OrderDetails.CancelEdit();
            OrderItemList.IsMultipleSelection = false;
            var selected = OrderList.SelectedItem;
            if (!OrderList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                    await PopulateOrderItems(selected);
                }
            }
            OrderDetails.Item = selected;
        }

        private async Task PopulateDetails(OrderModel selected)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                var model = await dataProvider.GetOrderAsync(selected.OrderID);
                selected.Merge(model);
            }
        }

        private async Task PopulateOrderItems(OrderModel selected)
        {
            if (selected != null)
            {
                await OrderItemList.LoadAsync(new OrderItemsViewState { OrderID = selected.OrderID });
            }
        }
    }
}
