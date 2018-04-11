﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomerListViewModel : ListViewModel<CustomerModel>
    {
        public CustomerListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        public CustomersViewState ViewState { get; private set; }

        public async Task LoadAsync(CustomersViewState state)
        {
            MessageService.Subscribe<CustomerDetailsViewModel>(this, OnMessage);
            MessageService.Subscribe<CustomerListViewModel>(this, OnMessage);

            ViewState = state ?? CustomersViewState.CreateDefault();
            ApplyViewState(ViewState);
            await RefreshAsync();
        }

        public void Unload()
        {
            UpdateViewState(ViewState);
            MessageService.Unsubscribe(this);
        }

        public override async void New()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<CustomerDetailsViewModel>(new CustomerViewState());
            }
            else
            {
                NavigationService.Navigate<CustomerDetailsViewModel>(new CustomerViewState());
            }
        }

        override public async Task<IList<CustomerModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<Customer>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            var virtualCollection = new CustomerCollection(ProviderFactory.CreateDataProvider());
            await virtualCollection.RefreshAsync(request);
            return virtualCollection;
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<CustomerModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteCustomerAsync(model);
            }
            MessageService.Send(this, "ItemsDeleted", models);
        }

        protected override async Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges)
        {
            var request = new DataRequest<Customer>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            foreach (var range in ranges)
            {
                await dataProvider.DeleteCustomerRangeAsync(range.Index, range.Length, request);
            }
            MessageService.Send(this, "ItemRangesDeleted", ranges);
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected customers?", "Ok", "Cancel");
        }

        public CustomersViewState GetCurrentState()
        {
            var state = CustomersViewState.CreateDefault();
            UpdateViewState(state);
            return state;
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
