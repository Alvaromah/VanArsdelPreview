using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    abstract public class ListViewModel<TModel> : ViewModelBase where TModel : ModelBase
    {
        public ListViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public override string Title => String.IsNullOrEmpty(Query) ? " " : $"results for \"{Query}\"";

        public string DataUnavailableMessage => Items == null ? "Loading..." : "No items found";

        public string Query { get; set; }

        public bool IsDataAvailable => (_items?.Count ?? 0) > 0;
        public bool IsDataUnavailable => !IsDataAvailable;

        private IList<TModel> _items;
        public IList<TModel> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        private TModel _selectedItem;
        public TModel SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private int _itemsCount = 0;
        public int ItemsCount
        {
            get => _itemsCount;
            set => Set(ref _itemsCount, value);
        }

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => _pageIndex;
            set { if (Set(ref _pageIndex, value)) Refresh(); }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => Set(ref _pageSize, value);
        }

        private async void Refresh(bool resetPageIndex = false)
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
            _itemsCount = page.Count;
            _pageIndex = page.PageIndex;

            RaiseUpdateView();
        }

        abstract public Task<PageResult<TModel>> GetItemsAsync(IDataProvider dataProvider);
    }
}
