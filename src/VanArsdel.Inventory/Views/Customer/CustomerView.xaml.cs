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
    public sealed partial class CustomerView : Page
    {
        public CustomerView()
        {
            ViewModel = new CustomerDetailsViewModel(new DataProviderFactory());
            InitializeComponent();
        }

        public CustomerDetailsViewModel ViewModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Main.HideBackButton();

            ViewModel.PropertyChanged += OnPropertyChanged;
            ViewModel.ItemDeleted += OnItemDeleted;

            var state = e.Parameter as CustomerViewState;
            state = state ?? CustomerViewState.CreateDefault();
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
            await ViewManager.Current.CreateNewView(typeof(CustomerView), new CustomerViewState { CustomerID = ViewModel.Item.CustomerID });
            NavigationService.Main.GoBack();
        }
    }
}
