using System;
using System.ComponentModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersView : Page
    {
        public CustomersView()
        {
            ViewModel = new CustomersViewModel(new DataProviderFactory());
            InitializeComponent();
        }

        public CustomersViewModel ViewModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.CustomerList.PropertyChanged += OnViewModelPropertyChanged;
            await ViewModel.LoadAsync(e.Parameter as CustomersViewState);
            UpdateTitle();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.CancelEdit();
            ViewModel.Unload();
            ViewModel.CustomerList.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CustomerList.Title))
            {
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            this.SetTitle($"Customers {ViewModel.CustomerList.Title}".Trim());
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
    }
}
