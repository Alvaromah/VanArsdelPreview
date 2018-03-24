using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Windows.Storage;

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
            ServiceLocator.Configure(_serviceCollection);

            NavigationService.Register<ShellViewModel, ShellView>();
            NavigationService.Register<MainShellViewModel, MainShellView>();
            NavigationService.Register<DashboardViewModel, DashboardView>();
            NavigationService.Register<CustomersViewModel, CustomersView>();
            NavigationService.Register<OrdersViewModel, OrdersView>();
            NavigationService.Register<ProductsViewModel, ProductsView>();
            NavigationService.Register<SettingsViewModel, SettingsView>();

            await EnsureDatabaseAsync();
        }

        static private async Task EnsureDatabaseAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var databaseFolder = await localFolder.CreateFolderAsync(AppSettings.DatabasePath, CreationCollisionOption.OpenIfExists);

            if (await databaseFolder.TryGetItemAsync(AppSettings.DatabaseName) == null)
            {
                if (await databaseFolder.TryGetItemAsync(AppSettings.DatabasePattern) == null)
                {
                    using (var cli = new WebClient())
                    {
                        var bytes = cli.DownloadData(AppSettings.DatabaseUrl);
                        var file = await databaseFolder.CreateFileAsync(AppSettings.DatabasePattern, CreationCollisionOption.ReplaceExisting);
                        using (var stream = await file.OpenStreamForWriteAsync())
                        {
                            await stream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
                var sourceFile = await databaseFolder.GetFileAsync(AppSettings.DatabasePattern);
                var targetFile = await databaseFolder.CreateFileAsync(AppSettings.DatabaseName, CreationCollisionOption.ReplaceExisting);
                await sourceFile.CopyAndReplaceAsync(targetFile);
            }
        }
    }
}
