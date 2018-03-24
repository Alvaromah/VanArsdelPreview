using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        public SettingsViewState ViewState { get; private set; }

        public Task LoadAsync(SettingsViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
