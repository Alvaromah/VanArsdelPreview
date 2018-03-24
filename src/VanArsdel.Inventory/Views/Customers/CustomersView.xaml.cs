using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersView : Page
    {
        public CustomersView()
        {
            ViewModel = ServiceLocator.Current.GetService<CustomersViewModel>();
            InitializeComponent();
        }

        public CustomersViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAsync(e.Parameter as CustomersViewState);
        }
    }
}
