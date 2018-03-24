using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        private string _statusMessage = null;
        public string StatusMessage
        {
            get => _statusMessage;
            set => Set(ref _statusMessage, value);
        }

        public ShellViewState ViewState { get; protected set; }

        virtual public Task LoadAsync(ShellViewState viewState)
        {
            ViewState = viewState ?? new ShellViewState();
            NavigationService.Navigate(ViewState.ViewModel, ViewState.Parameter);
            return Task.CompletedTask;
        }

        virtual public void Unload()
        {
        }
    }
}
