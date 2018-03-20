using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrderView : Page
    {
        public OrderView()
        {
            ViewModel = new OrderDetailsViewModel(new DataProviderFactory());
            InitializeComponent();
        }

        public OrderDetailsViewModel ViewModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Main.HideBackButton();

            ViewModel.PropertyChanged += OnPropertyChanged;
            ViewModel.ItemDeleted += OnItemDeleted;

            var state = e.Parameter as OrderViewState;
            state = state ?? OrderViewState.CreateDefault();
            await ViewModel.LoadAsync(state);
            UpdateTitle();

            if (state.IsNew)
            {
                await Task.Delay(100);
                details.SetFocus();
            }

            Bindings.Update();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            this.SetTitle(ViewModel.Title);
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await ViewManager.Current.Close();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.IsEditMode = false;
            await ViewManager.Current.CreateNewView(typeof(OrderView), new OrderViewState(ViewModel.Item.CustomerID) { OrderID = ViewModel.Item.OrderID });
            NavigationService.Main.GoBack();
        }
    }
}
