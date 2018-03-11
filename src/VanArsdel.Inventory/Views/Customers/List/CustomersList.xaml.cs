using System;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersList : UserControl
    {
        public CustomersList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public CustomerListViewModel ViewModel
        {
            get { return (CustomerListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerListViewModel), typeof(CustomersList), new PropertyMetadata(null));
        #endregion

        public ICommand NewCommand => new RelayCommand(New);
        private async void New()
        {
            await ViewManager.Current.CreateNewView(typeof(CustomerView), new CustomerViewState());
        }
    }
}
