using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrderItemView : Page
    {
        public OrderItemView()
        {
            ViewModel = new OrderItemDetailsViewModel(new DataProviderFactory());
            ViewModel.ItemDeleted += OnItemDeleted;
            InitializeComponent();
        }

        public OrderItemDetailsViewModel ViewModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Main.HideBackButton();

            var state = e.Parameter as OrderItemViewState;
            state = state ?? OrderItemViewState.CreateDefault();
            await ViewModel.LoadAsync(state);
            this.SetTitle(ViewModel.Title);

            if (state.IsNew)
            {
                await Task.Delay(100);
                details.SetFocus();
            }

            Bindings.Update();
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await ViewManager.Current.Close();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            // TODO: 
            //ViewModel.IsEditMode = false;
            //await ViewManager.Current.CreateNewView(typeof(OrderItemView), new OrderItemViewState { OrderItemID = ViewModel.Item.OrderItemID });
            //NavigationService.Main.GoBack();
        }
    }
}
