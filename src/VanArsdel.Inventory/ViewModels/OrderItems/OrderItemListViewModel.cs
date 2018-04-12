using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemListViewModel : ListViewModel<OrderItemModel>
    {
        public OrderItemListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        public OrderItemsViewState ViewState { get; private set; }

        public async Task LoadAsync(OrderItemsViewState state)
        {
            ViewState = state ?? OrderItemsViewState.CreateEmpty();
            Query = state.Query;
            await RefreshAsync();
        }

        public void Unload()
        {
            ViewState.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderItemDetailsViewModel>(this, OnMessage);
            MessageService.Subscribe<OrderItemListViewModel>(this, OnMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public override async void New()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<OrderItemDetailsViewModel>(new OrderItemViewState(ViewState.OrderID));
            }
            else
            {
                NavigationService.Navigate<OrderItemDetailsViewModel>(new OrderItemViewState(ViewState.OrderID));
            }
        }

        override public async Task<IList<OrderItemModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<OrderItem>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            if (ViewState.OrderID > 0)
            {
                request.Where = (r) => r.OrderID == ViewState.OrderID;
            }
            return await dataProvider.GetOrderItemsAsync(0, -1, request);
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<OrderItemModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteOrderItemAsync(model);
            }
        }

        protected override Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges)
        {
            throw new NotImplementedException();
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected order items?", "Ok", "Cancel");
        }

        public OrderItemsViewState GetCurrentState()
        {
            return new OrderItemsViewState
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc,
                OrderID = ViewState.OrderID
            };
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case "ItemChanged":
                case "ItemDeleted":
                case "ItemsDeleted":
                case "ItemRangesDeleted":
                    await Dispatcher.RunIdleAsync((e) =>
                    {
                        Refresh();
                    });
                    break;
            }
        }
    }
}
