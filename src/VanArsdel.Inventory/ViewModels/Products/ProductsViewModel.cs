using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        public ProductsViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        public ProductsViewState ViewState { get; private set; }

        public Task LoadAsync(ProductsViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
