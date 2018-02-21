using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomerOrdersView : Page
    {
        public CustomerOrdersView()
        {
            InitializeViewModel();
            InitializeComponent();
        }

        public CustomerOrdersViewModel ViewModel { get; private set; }

        private void InitializeViewModel()
        {
            ViewModel = new CustomerOrdersViewModel(new DataProviderFactory());
            ViewModel.UpdateBindings += OnUpdateBindings;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ShellView.Current.IsPaneOpen = false;
            await Task.Delay(100);
            await ViewModel.LoadAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
        }

        private void OnUpdateBindings(object sender, EventArgs e)
        {
            Bindings.Update();
        }

        private void OnCustomerQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.RefreshCustomers(resetPageIndex: true);
        }

        private void OnOrderQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.RefreshOrders(resetPageIndex: true);
        }

        private async void OnDetailButtonClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.Delete:
                    if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current customer?", "Ok", "Cancel"))
                    {
                        await ViewModel.DeleteCurrentOrderAsync();
                    }
                    break;
            }
        }
    }
}
