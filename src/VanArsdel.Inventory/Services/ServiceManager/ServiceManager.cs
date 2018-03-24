using System;

namespace VanArsdel.Inventory.Services
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(NavigationService navigationService, MessageService messageService, DialogService dialogService, LogService logService)
        {
            NavigationService = navigationService;
            MessageService = messageService;
            DialogService = dialogService;
            LogService = logService;
        }

        public INavigationService NavigationService { get; }

        public IMessageService MessageService { get; }

        public IDialogService DialogService { get; }

        public ILogService LogService { get; }
    }
}
