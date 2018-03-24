using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using VanArsdel.Inventory.Views;
using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory
{
    static public class Startup
    {
        static private readonly ServiceCollection _serviceCollection = new ServiceCollection();

        static public async Task ConfigureAsync()
        {
            await Task.Delay(500);

            ServiceLocator.Configure(_serviceCollection);

            NavigationService.Register<ShellViewModel, ShellView>();
            NavigationService.Register<MainShellViewModel, MainShellView>();
            NavigationService.Register<DashboardViewModel, DashboardView>();
            NavigationService.Register<CustomersViewModel, CustomersView>();
            NavigationService.Register<OrdersViewModel, OrdersView>();
            NavigationService.Register<ProductsViewModel, ProductsView>();
            NavigationService.Register<SettingsViewModel, SettingsView>();
        }
    }
}
