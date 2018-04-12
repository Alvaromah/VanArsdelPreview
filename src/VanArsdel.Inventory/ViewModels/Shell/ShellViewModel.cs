using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(IDataProviderFactory providerFactory, IContext context, INavigationService navigationService, IMessageService messageService) : base(context)
        {
            ProviderFactory = providerFactory;
            NavigationService = navigationService;
            MessageService = messageService;
        }

        public IDataProviderFactory ProviderFactory { get; }
        public INavigationService NavigationService { get; }
        public IMessageService MessageService { get; }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        private string _statusMessage = "Ready";
        public string StatusMessage
        {
            get => _statusMessage;
            set => Set(ref _statusMessage, value);
        }

        private bool _isError = false;
        public bool IsError
        {
            get => _isError;
            set => Set(ref _isError, value);
        }

        public ShellViewState ViewState { get; protected set; }

        virtual public async Task LoadAsync(ShellViewState viewState)
        {
            // TODOX: Implement as a Scoped service?
            await DataHelper.Current.InitializeAsync(ProviderFactory);

            ViewState = viewState ?? new ShellViewState();
            NavigationService.Navigate(ViewState.ViewModel, ViewState.Parameter);
        }

        virtual public void Unload()
        {
        }

        public void Subscribe()
        {
            MessageService.Subscribe<ViewModelBase>(this, OnMessage);
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            var viewModel = sender as ViewModelBase;
            if (viewModel.Context.ViewID != Context.ViewID)
            {
                return;
            }

            switch (message)
            {
                case "StatusMessage":
                case "StatusError":
                    await Dispatcher.RunIdleAsync((e) =>
                    {
                        IsError = message == "StatusError";
                        StatusMessage = args.ToString();
                    });
                    break;
            }
        }
    }
}
