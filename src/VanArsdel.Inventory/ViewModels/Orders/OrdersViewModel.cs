using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public OrdersViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        public OrdersViewState ViewState { get; private set; }

        public Task LoadAsync(OrdersViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
