using System;

namespace VanArsdel.Inventory.Services
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(IContext context, INavigationService navigationService, IMessageService messageService, IDialogService dialogService, ILogService logService)
        {
            Context = context;
            NavigationService = navigationService;
            MessageService = messageService;
            DialogService = dialogService;
            LogService = logService;
        }

        public IContext Context { get; }

        public INavigationService NavigationService { get; }

        public IMessageService MessageService { get; }

        public IDialogService DialogService { get; }

        public ILogService LogService { get; }
    }
}
