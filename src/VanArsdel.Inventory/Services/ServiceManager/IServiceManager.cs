using System;

namespace VanArsdel.Inventory.Services
{
    public interface IServiceManager
    {
        INavigationService NavigationService { get; }
        IMessageService MessageService { get; }
        IDialogService DialogService { get; }
        ILogService LogService { get; }
    }
}
