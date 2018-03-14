using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Providers;
using VanArsdel.Inventory.ViewModels;
using System.Threading.Tasks;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersDialog : ContentDialog
    {
        public CustomersDialog()
        {
            ViewModel = new CustomerListViewModel(new DataProviderFactory());
            InitializeComponent();
        }

        #region ViewModel
        public CustomerListViewModel ViewModel
        {
            get { return (CustomerListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CustomerListViewModel), typeof(CustomersDialog), new PropertyMetadata(null));
        #endregion

        public long CustomerID { get; private set; }

        public async Task LoadAsync()
        {
            await ViewModel.LoadAsync(CustomersViewState.CreateDefault());
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CustomerID = ViewModel.SelectedItem?.CustomerID ?? 0;
        }

        private void OnSecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CustomerID = -1;
        }
    }
}
