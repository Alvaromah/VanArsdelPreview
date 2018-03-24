using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        public CustomersViewState ViewState { get; private set; }

        public Task LoadAsync(CustomersViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
