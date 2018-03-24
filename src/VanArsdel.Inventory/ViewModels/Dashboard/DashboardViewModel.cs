using System;
using System.Threading.Tasks;
using System.Windows.Input;
using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        public DashboardViewState ViewState { get; private set; }

        public Task LoadAsync(DashboardViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }


        public ICommand OpenNewCommand => new RelayCommand(OpenNew);
        private async void OpenNew()
        {
            await NavigationService.CreateNewViewAsync<CustomersViewModel>();
        }
    }
}
