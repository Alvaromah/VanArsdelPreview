﻿using System;
using System.Linq;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Core;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class MainShellView : Page
    {
        private INavigationService _navigationService = null;

        public MainShellView()
        {
            ViewModel = ServiceLocator.Current.GetService<MainShellViewModel>();
            InitializeContext();
            InitializeComponent();
            InitializeNavigation();
        }

        public MainShellViewModel ViewModel { get; }

        private SystemNavigationManager CurrentView => SystemNavigationManager.GetForCurrentView();

        private void InitializeContext()
        {
            var context = ServiceLocator.Current.GetService<IContext>();
            context.Initialize(ApplicationView.GetForCurrentView().Id, Dispatcher);
        }

        private void InitializeNavigation()
        {
            _navigationService = ServiceLocator.Current.GetService<INavigationService>();
            _navigationService.Initialize(frame);
            frame.Navigated += OnFrameNavigated;
            CurrentView.BackRequested += OnBackRequested;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync(e.Parameter as ShellViewState);
            ViewModel.Subscribe();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
            ViewModel.Unsubscribe();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationItem item)
            {
                ViewModel.NavigateTo(item.ViewModel);
            }
            else if (args.IsSettingsSelected)
            {
                ViewModel.NavigateTo(typeof(SettingsViewModel));
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
                e.Handled = true;
            }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            var targetType = NavigationService.GetViewModel(e.SourcePageType);
            switch (targetType.Name)
            {
                case "SettingsViewModel":
                    ViewModel.SelectedItem = navigationView.SettingsItem;
                    break;
                default:
                    ViewModel.SelectedItem = ViewModel.Items.Where(r => r.ViewModel == targetType).FirstOrDefault();
                    break;
            }
            CurrentView.AppViewBackButtonVisibility = _navigationService.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
