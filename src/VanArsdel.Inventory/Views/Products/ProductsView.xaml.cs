using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class ProductsView : Page
    {
        public ProductsView()
        {
            ViewModel = ServiceLocator.Current.GetService<ProductsViewModel>();
            InitializeComponent();
        }

        public ProductsViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadAsync(e.Parameter as ProductsViewState);
        }
    }
}
