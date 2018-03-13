using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;
using VanArsdel.Inventory.Views;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomerListViewModel : ListViewModel<CustomerModel>
    {
        public CustomerListViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
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
                await ViewManager.Current.CreateNewView(typeof(CustomerView), new CustomerViewState());
            }
            else
            {
                NavigationService.Main.Navigate(typeof(CustomerView), new CustomerViewState());
            }
        }

        override public async Task<PageResult<CustomerModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<Customer>(PageIndex, PageSize)
            {
                Query = Query,
                OrderBy = r => r.FirstName
            };
            return await dataProvider.GetCustomersAsync(request);
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<CustomerModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteCustomerAsync(model);
            }
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete selected customers?", "Ok", "Cancel");
        }

        public CustomersViewState GetCurrentState()
        {
            var state = CustomersViewState.CreateDefault();
            UpdateViewState(state);
            return state;
        }
    }
}
