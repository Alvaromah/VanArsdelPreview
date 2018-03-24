using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    abstract public partial class ListViewModel<TModel> : ViewModelBase where TModel : ModelBase
    {
        public ListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;
            NavigationService = serviceManager.NavigationService;
            MessageService = serviceManager.MessageService;
            DialogService = serviceManager.DialogService;
            LogService = serviceManager.LogService;
        }

        public IDataProviderFactory ProviderFactory { get; }
        public INavigationService NavigationService { get; }
        public IMessageService MessageService { get; }
        public IDialogService DialogService { get; }
        public ILogService LogService { get; }

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

            var page = await GetItemsAsync(dataProvider);
            Items = page.Items;
            SelectedItem = Items.FirstOrDefault();
            ItemsCount = page.Count;

            // Update dependent properties
            NotifyPropertyChanged(nameof(Title));
            NotifyPropertyChanged(nameof(IsDataAvailable));

            // Update PageIndex preventing firing Refresh() again
            Set(ref _pageIndex, page.PageIndex, nameof(PageIndex));
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
