using System;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrdersList : UserControl
    {
        public OrdersList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderListViewModel ViewModel
        {
            get { return (OrderListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderListViewModel), typeof(OrdersList), new PropertyMetadata(null));
        #endregion

        public ICommand NewCommand => new RelayCommand(New);
        private async void New()
        {
            await ViewManager.Current.CreateNewView(typeof(OrderView), new CustomerViewState());
        }
    }
}
