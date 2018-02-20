using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

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
            ViewModel.UpdateBindings += OnUpdateBindings;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await Task.Delay(100);
            await ViewModel.LoadAsync();
        }

        private void OnUpdateBindings(object sender, EventArgs e)
        {
            Bindings.Update();
        }

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Refresh(resetPageIndex: true);
        }

        private async void OnDetailButtonClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.Delete:
                    if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current order?", "Ok", "Cancel"))
                    {
                        await ViewModel.DeleteCurrentAsync();
                    }
                    break;
            }
        }
    }
}
