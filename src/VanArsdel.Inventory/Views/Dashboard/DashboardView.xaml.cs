using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class DashboardView : Page
    {
        public DashboardView()
        {
            ViewModel = ServiceLocator.Current.GetService<DashboardViewModel>();
            InitializeComponent();
        }

        public DashboardViewModel ViewModel { get; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync(e.Parameter as DashboardViewState);
        }
    }
}
