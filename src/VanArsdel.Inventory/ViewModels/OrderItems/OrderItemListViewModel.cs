﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;
using VanArsdel.Inventory.Views;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemListViewModel : ListViewModel<OrderItemModel>
    {
        public OrderItemListViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        public OrderItemsViewState ViewState { get; private set; }

        public async Task LoadAsync(OrderItemsViewState state)
        {
            ViewState = state ?? OrderItemsViewState.CreateDefault();
            ApplyViewState(ViewState);
            await RefreshAsync();
        }

        public void Unload()
        {
            UpdateViewState(ViewState);
        }

        public override async void New()
        {
            if (IsMainView)
            {
                await ViewManager.Current.CreateNewView(typeof(OrderItemView), new OrderItemViewState(ViewState.OrderID));
            }
            else
            {
                NavigationService.Main.Navigate(typeof(OrderItemView), new OrderItemViewState(ViewState.OrderID));
            }
        }

        override public async Task<PageResult<OrderItemModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<OrderItem>(PageIndex, PageSize)
            {
                Query = Query,
                OrderBy = r => r.OrderLine
            };
            if (ViewState.OrderID > 0)
            {
                request.Where = (r) => r.OrderID == ViewState.OrderID;
            }
            return await dataProvider.GetOrderItemsAsync(request);
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<OrderItemModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteOrderItemAsync(model);
            }
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete selected order items?", "Ok", "Cancel");
        }

        public CustomersViewState GetCurrentState()
        {
            var state = CustomersViewState.CreateDefault();
            UpdateViewState(state);
            return state;
        }
    }
}
