using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Services;
using Windows.UI.ViewManagement;

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
            //frame.Navigate(typeof(CustomersView));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
        }

        private void OnViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            ViewModel = null;
            Bindings.StopTracking();
            frame.Navigate(typeof(Page));
            //var appView = ApplicationView.GetForCurrentView();
            //appView.Consolidated -= OnViewConsolidated;
            ServiceLocator.DisposeCurrent();
        }
    }
}
