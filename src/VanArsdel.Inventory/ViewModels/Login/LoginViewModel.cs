using System;
using System.Threading.Tasks;
using System.Windows.Input;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        public LoginViewState ViewState { get; private set; }

        public Task LoadAsync(LoginViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
