﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class ProductListViewModel : ListViewModel<ProductModel>
    {
        public ProductListViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        public ProductsViewState ViewState { get; private set; }

        public ICommand ItemInvokedCommand => new RelayCommand<ProductModel>(ItemInvoked);
        private async void ItemInvoked(ProductModel model)
        {
            await NavigationService.CreateNewViewAsync<ProductDetailsViewModel>(new ProductViewState { ProductID = model.ProductID });
        }

        public async Task LoadAsync(ProductsViewState state)
        {
            ViewState = state ?? ProductsViewState.CreateEmpty();
            Query = state.Query;
            await RefreshAsync();
        }

        public void Unload()
        {
            ViewState.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ProductDetailsViewModel>(this, OnMessage);
            MessageService.Subscribe<ProductListViewModel>(this, OnMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public override async void New()
        {
            if (IsMainView)
            {
                await NavigationService.CreateNewViewAsync<ProductDetailsViewModel>(new ProductViewState());
            }
            else
            {
                NavigationService.Navigate<ProductDetailsViewModel>(new ProductViewState());
            }
        }

        override public async Task<IList<ProductModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new DataRequest<Product>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            var virtualCollection = new ProductCollection(ProviderFactory.CreateDataProvider());
            await virtualCollection.RefreshAsync(request);
            return virtualCollection;
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<ProductModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteProductAsync(model);
            }
        }

        protected override async Task DeleteRangesAsync(IDataProvider dataProvider, IEnumerable<IndexRange> ranges)
        {
            var request = new DataRequest<Product>()
            {
                Query = Query,
                OrderBy = ViewState.OrderBy,
                OrderByDesc = ViewState.OrderByDesc
            };
            foreach (var range in ranges)
            {
                await dataProvider.DeleteProductRangeAsync(range.Index, range.Length, request);
            }
        }

        protected override async Task<bool> ConfirmDeleteSelectionAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete selected products?", "Ok", "Cancel");
        }

        public ProductsViewState GetCurrentState()
        {
            return new ProductsViewState
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
                    await Dispatcher.RunIdleAsync((e) =>
                    {
                        Refresh();
                    });
                    break;
            }
        }
    }
}
