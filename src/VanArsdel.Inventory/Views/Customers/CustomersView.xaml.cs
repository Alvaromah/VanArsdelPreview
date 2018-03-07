using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;
using Windows.UI.Xaml;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersView : Page
    {
        public CustomersView()
        {
            InitializeViewModel();
            InitializeComponent();
        }

        public CustomersViewModel ViewModel { get; private set; }

        private void InitializeViewModel()
        {
            ViewModel = new CustomersViewModel(new DataProviderFactory());
            ViewModel.UpdateView += OnUpdateView;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.SetTitle("Customers");
            await ViewModel.LoadAsync(e.Parameter as CustomersViewState);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.CancelEdit();
            ViewModel.SaveState();
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await ViewModel.RefreshAsync();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            await ViewManager.Current.CreateNewView(typeof(CustomersView), ViewModel.CustomerList.GetCurrentState());
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.CustomerDetails.IsEditMode = false;
            if (pivot.SelectedIndex == 0)
            {
                await ViewManager.Current.CreateNewView(typeof(CustomerView), new CustomerViewState { CustomerID = ViewModel.CustomerDetails.Item.CustomerID });
            }
            else
            {
                await ViewManager.Current.CreateNewView(typeof(OrdersView), ViewModel.CustomerOrders.ViewState.Clone());
            }
        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }

        public string GetSelectionText(int count)
        {
            return $"{count} items selected.";
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
