using System;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersOrders : UserControl
    {
        public CustomersOrders()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderListViewModel ViewModel
        {
            get { return (OrderListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(OrderListViewModel), typeof(CustomersOrders), new PropertyMetadata(null));
        #endregion

        public ICommand NewCommand => new RelayCommand(New);
        private void New()
        {
            // TODO: 
            //await ViewManager.Current.CreateNewView(typeof(OrderView), new OrderViewState());
        }
    }
}
