using System;
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

        public async Task LoadAsync()
        {
            await base.RefreshAsync();
        }

        override public async Task<PageResult<CustomerModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            return await dataProvider.GetCustomersAsync(PageIndex, PageSize, Query, r => r.FirstName);
        }
    }
}
