﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    abstract public partial class ListViewModel<TModel> : ViewModelBase where TModel : ModelBase
    {
        public ListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager) : base(serviceManager.Context)
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

        public async Task RefreshAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await RefreshAsync(dataProvider);
                SelectedItem = Items.FirstOrDefault();
            }
        }

        virtual protected async Task RefreshAsync(IDataProvider dataProvider)
        {
            Items = null;
            SelectedItem = null;

            Items = await GetItemsAsync(dataProvider);
            ItemsCount = Items.Count;

            NotifyPropertyChanged(nameof(Title));
            NotifyPropertyChanged(nameof(IsDataAvailable));
        }

        public void StatusReady()
        {
            MessageService.Send(this, "StatusMessage", "Ready");
        }

        public void StatusMessage(string message)
        {
            MessageService.Send(this, "StatusMessage", message);
        }

        public void StatusError(string message)
        {
            MessageService.Send(this, "StatusError", message);
        }

        abstract public Task<IList<TModel>> GetItemsAsync(IDataProvider dataProvider);
        abstract protected Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<TModel> models);
        abstract protected Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges);
    }
}
