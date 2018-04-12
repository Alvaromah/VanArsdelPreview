﻿using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class ShellView : Page
    {
        public ShellView()
        {
            ViewModel = ServiceLocator.Current.GetService<ShellViewModel>();
            InitializeComponent();
            InitializeNavigation();
        }

        public ShellViewModel ViewModel { get; private set; }

        private void InitializeNavigation()
        {
            var navigationService = ServiceLocator.Current.GetService<INavigationService>();
            navigationService.Initialize(frame);
            var appView = ApplicationView.GetForCurrentView();
            appView.Consolidated += OnViewConsolidated;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync(e.Parameter as ShellViewState);
            ViewModel.Subscribe();
        }

        private void OnViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            ViewModel.Unsubscribe();
            ViewModel = null;
            Bindings.StopTracking();
            frame.Navigate(typeof(Page));
            var appView = ApplicationView.GetForCurrentView();
            appView.Consolidated -= OnViewConsolidated;
            ServiceLocator.DisposeCurrent();
        }
    }
}
