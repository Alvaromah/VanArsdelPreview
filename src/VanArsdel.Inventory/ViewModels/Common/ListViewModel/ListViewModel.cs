using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Controls;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    abstract public partial class ListViewModel<TModel> : ViewModelBase where TModel : ModelBase
    {
        public ListViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }


        private async void Refresh(bool resetPageIndex)
        {
            await RefreshAsync(resetPageIndex);
        }

        public async Task RefreshAsync(bool resetPageIndex = false)
        {
            if (resetPageIndex)
            {
                _pageIndex = 0;
            }
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await RefreshAsync(dataProvider);
            }
        }

        private async Task RefreshAsync(IDataProvider dataProvider)
        {
            Items = null;
            SelectedItem = null;

            //var page = await dataProvider.GetCustomersAsync(PageIndex, 10, Query, r => r.FirstName);
            var page = await GetItemsAsync(dataProvider);

            // First, update Customer list
            Items = page.Items;

            // Then, update selected Customer
            SelectedItem = Items.FirstOrDefault();

            // Finally update other properties, preventing firing Refresh() again
            ItemsCount = page.Count;
            _pageIndex = page.PageIndex;

            RaiseUpdateView();
        }

        public async Task DeleteSelectionAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await DeleteItemsAsync(dataProvider, _selectedItems);
                await RefreshAsync(dataProvider);
            }
        }

        virtual public void ApplyViewState(ListViewState state)
        {
            _pageIndex = state.PageIndex;
            _pageSize = state.PageSize;
            Query = state.Query;
        }

        virtual public void UpdateViewState(ListViewState state)
        {
            state.PageIndex = PageIndex;
            state.PageSize = PageSize;
            state.Query = Query;
        }

        abstract public Task<PageResult<TModel>> GetItemsAsync(IDataProvider dataProvider);
        abstract protected Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<TModel> models);
    }
}
