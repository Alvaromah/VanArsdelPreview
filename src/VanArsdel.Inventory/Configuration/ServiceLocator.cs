﻿using System;
using System.Collections.Concurrent;

using Windows.UI.ViewManagement;

using Microsoft.Extensions.DependencyInjection;

using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory
{
    public class ServiceLocator : IDisposable
    {
        static private readonly ConcurrentDictionary<int, ServiceLocator> _serviceLocators = new ConcurrentDictionary<int, ServiceLocator>();

        static private ServiceProvider _rootServiceProvider = null;

        static public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDataProviderFactory, DataProviderFactory>();
            serviceCollection.AddSingleton<IMessageService, MessageService>();
            serviceCollection.AddSingleton<ILogService, LogService>();

            serviceCollection.AddScoped<INavigationService, NavigationService>();
            serviceCollection.AddScoped<IDialogService, DialogService>();
            serviceCollection.AddScoped<IServiceManager, ServiceManager>();

            serviceCollection.AddTransient<ShellViewModel>();
            serviceCollection.AddTransient<MainShellViewModel>();

            serviceCollection.AddTransient<DashboardViewModel>();

            serviceCollection.AddTransient<CustomersViewModel>();
            serviceCollection.AddTransient<CustomerDetailsViewModel>();

            serviceCollection.AddTransient<OrdersViewModel>();
            serviceCollection.AddTransient<OrderDetailsViewModel>();

            serviceCollection.AddTransient<OrderItemsViewModel>();
            serviceCollection.AddTransient<OrderItemDetailsViewModel>();

            serviceCollection.AddTransient<ProductsViewModel>();
            serviceCollection.AddTransient<SettingsViewModel>();

            _rootServiceProvider = serviceCollection.BuildServiceProvider();
        }

        static public ServiceLocator Current
        {
            get
            {
                int currentViewId = ApplicationView.GetForCurrentView().Id;
                return _serviceLocators.GetOrAdd(currentViewId, key => new ServiceLocator());
            }
        }

        static public void DisposeCurrent()
        {
            int currentViewId = ApplicationView.GetForCurrentView().Id;
            if (_serviceLocators.TryRemove(currentViewId, out ServiceLocator current))
            {
                current.Dispose();
            }
        }

        private IServiceScope _serviceScope = null;

        private ServiceLocator()
        {
            _serviceScope = _rootServiceProvider.CreateScope();
        }

        public T GetService<T>()
        {
            return GetService<T>(true);
        }

        public T GetService<T>(bool isRequired)
        {
            if (isRequired)
            {
                return _serviceScope.ServiceProvider.GetRequiredService<T>();
            }
            return _serviceScope.ServiceProvider.GetService<T>();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_serviceScope != null)
                {
                    _serviceScope.Dispose();
                }
            }
        }
        #endregion
    }
}
