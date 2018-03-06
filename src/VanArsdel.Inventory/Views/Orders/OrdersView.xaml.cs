using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;
using Windows.UI.Xaml;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrdersView : Page
    {
        public OrdersView()
        {
            InitializeViewModel();
            InitializeComponent();
        }

        public OrdersViewModel ViewModel { get; private set; }

        private void InitializeViewModel()
        {
            ViewModel = new OrdersViewModel(new DataProviderFactory());
            ViewModel.UpdateView += OnUpdateView;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.SetTitle("Orders");
            await ViewModel.LoadAsync(e.Parameter as OrdersViewState);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.CancelEdit();
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await ViewModel.RefreshAsync();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            //ViewModel.OrderDetails.IsEditMode = false;
            //await ViewManager.Current.CreateNewView(typeof(OrderView), new OrderViewState { OrderID = ViewModel.OrderDetails.Item.OrderID });
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
