using System;
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
            ViewState = state ?? CustomersViewState.CreateEmpty();
            Query = state.Query;

            StatusMessage("Loading customers...");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await RefreshAsync();
            stopwatch.Stop();
            StatusMessage($"Customers loaded  ({stopwatch.Elapsed.TotalSeconds:#0.00} seconds)");
        }

        public void Unload()
        {
            ViewState.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CustomerDetailsViewModel>(this, OnMessage);
            MessageService.Subscribe<CustomerListViewModel>(this, OnMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        protected override async void Refresh()
        {
            StatusMessage("Searching customers...");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await RefreshAsync();
            stopwatch.Stop();
            StatusMessage($"Customers search results ({stopwatch.Elapsed.TotalSeconds:0.00} seconds)");
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
            if (ViewState.IsEmpty)
            {
                return new List<CustomerModel>();
            }

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
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected customers?", "Ok", "Cancel");
        }

        public CustomersViewState GetCurrentState()
        {
            return new CustomersViewState
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
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
                    await Dispatcher.RunIdleAsync(async (e) =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }
    }
}
