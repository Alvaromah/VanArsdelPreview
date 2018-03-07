using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomerListViewModel : ListViewModel<CustomerModel>
    {
        public CustomerListViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        public CustomersViewState ViewState { get; private set; }

        public async Task LoadAsync(CustomersViewState state = null)
        {
            ViewState = state ?? CustomersViewState.CreateDefault();
            await base.RefreshAsync();
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
    }
}
