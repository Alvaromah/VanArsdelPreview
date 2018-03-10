using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;
using System.Windows.Input;

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

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomersList;
            control.UpdateViewModel(e.OldValue as CustomerListViewModel, e.NewValue as CustomerListViewModel);
        }

        private void UpdateViewModel(CustomerListViewModel oldViewModel, CustomerListViewModel newViewModel)
        {
            if (oldViewModel != null)
            {
                oldViewModel.UpdateView -= OnUpdateView;
            }
            newViewModel.UpdateView += OnUpdateView;
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerListViewModel), typeof(CustomersList), new PropertyMetadata(null, ViewModelChanged));
        #endregion

        public ICommand NewCustomerCommand => new RelayCommand(NewCustomer);
        private async void NewCustomer()
        {
            await ViewManager.Current.CreateNewView(typeof(CustomerView), new CustomerViewState());
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
