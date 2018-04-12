using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderListViewModel : ListViewModel<OrderModel>
    {
        public OrderListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        public OrdersViewState ViewState { get; private set; }

        public async Task LoadAsync(OrdersViewState state)
        {
            ViewState = state ?? OrdersViewState.CreateEmpty();
            Query = state.Query;
            await RefreshAsync();
        }

        public void Unload()
        {
            ViewState.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDetailsViewModel>(this, OnMessage);
            MessageService.Subscribe<OrderListViewModel>(this, OnMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public override async void New()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(new OrderViewState(ViewState.CustomerID));
            }
            else
            {
                NavigationService.Navigate<OrderDetailsViewModel>(new OrderViewState(ViewState.CustomerID));
            }
        }

        override public async Task<IList<OrderModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            if (ViewState.IsEmpty)
            {
                return new List<OrderModel>();
            }

            var request = new DataRequest<Order>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            if (ViewState.CustomerID > 0)
            {
                request.Where = (r) => r.CustomerID == ViewState.CustomerID;
            }
            var virtualCollection = new OrderCollection(ProviderFactory.CreateDataProvider());
            await virtualCollection.RefreshAsync(request);
            return virtualCollection;
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<OrderModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteOrderAsync(model);
            }
        }

        protected override async Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges)
        {
            var request = new DataRequest<Order>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            foreach (var range in ranges)
            {
                await dataProvider.DeleteOrderRangeAsync(range.Index, range.Length, request);
            }
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected orders?", "Ok", "Cancel");
        }

        public OrdersViewState GetCurrentState()
        {
            return new OrdersViewState
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc,
                CustomerID = ViewState.CustomerID
            };
        }

        private async void OnMessage(object sender, string message, object args)
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
