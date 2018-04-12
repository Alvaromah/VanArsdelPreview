using System;
using System.ComponentModel;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemsViewModel : ViewModelBase
    {
        public OrderItemsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager) : base(serviceManager.Context)
        {
            ProviderFactory = providerFactory;
            MessageService = serviceManager.MessageService;

            OrderItemList = new OrderItemListViewModel(ProviderFactory, serviceManager);
            OrderItemDetails = new OrderItemDetailsViewModel(ProviderFactory, serviceManager);
        }

        public IDataProviderFactory ProviderFactory { get; }
        public IMessageService MessageService { get; }

        public OrderItemListViewModel OrderItemList { get; set; }
        public OrderItemDetailsViewModel OrderItemDetails { get; set; }

        public async Task LoadAsync(OrderItemsViewState state)
        {
            await OrderItemList.LoadAsync(state);
        }

        public void Unload()
        {
            OrderItemList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderItemListViewModel>(this, OnMessage);
            OrderItemList.Subscribe();
            OrderItemDetails.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderItemList.Unsubscribe();
            OrderItemDetails.Unsubscribe();
        }

        public async Task RefreshAsync()
        {
            await OrderItemList.RefreshAsync();
        }

        public void CancelEdit()
        {
            OrderItemDetails.CancelEdit();
        }

        private async void OnMessage(object sender, string message, object args)
        {
            if (sender == OrderItemList && message == "ItemSelected")
            {
                await Dispatcher.RunIdleAsync((e) =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            OrderItemDetails.CancelEdit();
            var selected = OrderItemList.SelectedItem;
            if (!OrderItemList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            OrderItemDetails.Item = selected;
        }

        private async Task PopulateDetails(OrderItemModel selected)
        {
            if (selected != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    var model = await dataProvider.GetOrderItemAsync(selected.OrderID, selected.OrderLine);
                    selected.Merge(model);
                }
            }
            OrderItemDetails.Item = selected;
        }
    }
}
