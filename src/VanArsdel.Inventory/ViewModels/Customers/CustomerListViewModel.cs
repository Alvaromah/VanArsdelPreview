using System;
using System.Linq;
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
        private CustomerCollection _virtualCollection = null;

        public CustomerListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
            _virtualCollection = new CustomerCollection(ProviderFactory);
        }

        public CustomersViewState ViewState { get; private set; }

        public async Task LoadAsync(CustomersViewState state)
        {
            ViewState = state ?? CustomersViewState.CreateDefault();
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
                await NavigationService.CreateNewViewAsync<CustomerDetailsViewModel>(new CustomerViewState());
            }
            else
            {
                NavigationService.Navigate<CustomerDetailsViewModel>(new CustomerViewState());
            }
        }

        protected override async Task RefreshAsync(IDataProvider dataProvider)
        {
            Items = null;
            SelectedItem = null;

            var request = new DataRequest<Customer>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            await _virtualCollection.RefreshAsync(request);

            Items = _virtualCollection;
            SelectedItem = Items.FirstOrDefault();
            ItemsCount = Items.Count;

            // Update dependent properties
            NotifyPropertyChanged(nameof(Title));
            NotifyPropertyChanged(nameof(IsDataAvailable));

            // Update PageIndex preventing firing Refresh() again
            //Set(ref _pageIndex, page.PageIndex, nameof(PageIndex));
        }

        override public Task<PageResult<CustomerModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            throw new NotImplementedException();
            //var request = new PageRequest<Customer>(PageIndex, PageSize)
            //{
            //    Query = Query,
            //    OrderBy = ViewState.OrderBy,
            //    OrderByDesc = ViewState.OrderByDesc
            //};
            //var virtualized = new CustomerVirtualList(ProviderFactory);
            //await virtualized.InitializeAsync(request);
            //return new PageResult<CustomerModel>(request.PageIndex, request.PageSize, virtualized.Count) { Items = virtualized };
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<CustomerModel> models)
        {
            var ids = new List<long>();

            using (var provider = ProviderFactory.CreateDataProvider())
            {
                foreach (var range in _virtualCollection.GetSelectedRanges())
                {
                    var request = new DataRequest<Customer>()
                    {
                        Query = Query,
                        OrderBy = ViewState.OrderBy,
                        OrderByDesc = ViewState.OrderByDesc
                    };
                    var items = (await provider.GetCustomersAsync(range.FirstIndex, (int)range.Length, request)).Items;
                    foreach (var item in items)
                    {
                        await dataProvider.DeleteCustomerAsync(item);
                    }
                }
            }

            //foreach (var model in models)
            //{
            //    System.Diagnostics.Debug.WriteLine("Delete {0}", model.FullName);
            //    await dataProvider.DeleteCustomerAsync(model);
            //}
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
    }
}
