using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        public ProductsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;
            MessageService = serviceManager.MessageService;

            ProductList = new ProductListViewModel(ProviderFactory, serviceManager);
            ProductDetails = new ProductDetailsViewModel(ProviderFactory, serviceManager);
        }

        public IDataProviderFactory ProviderFactory { get; }
        public IMessageService MessageService { get; }

        public ProductListViewModel ProductList { get; set; }
        public ProductDetailsViewModel ProductDetails { get; set; }

        public async Task LoadAsync(ProductsViewState state)
        {
            await ProductList.LoadAsync(state);
        }

        public void Unload()
        {
            ProductList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ProductListViewModel>(this, OnMessage);
            ProductList.Subscribe();
            ProductDetails.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            ProductList.Unsubscribe();
            ProductDetails.Unsubscribe();
        }

        public async Task RefreshAsync()
        {
            await ProductList.RefreshAsync();
        }

        public void CancelEdit()
        {
            ProductDetails.CancelEdit();
        }

        private async void OnMessage(object sender, string message, object args)
        {
            if (sender == ProductList && message == "ItemSelected")
            {
                await Dispatcher.RunIdleAsync((e) =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            ProductDetails.CancelEdit();
            var selected = ProductList.SelectedItem;
            if (!ProductList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                }
            }
            ProductDetails.Item = selected;
        }

        private async Task PopulateDetails(ProductModel selected)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                var model = await dataProvider.GetProductAsync(selected.ProductID);
                selected.Merge(model);
            }
        }
    }
}
