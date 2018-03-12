using System;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrderOrderItems : UserControl
    {
        public OrderOrderItems()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderItemListViewModel ViewModel
        {
            get { return (OrderItemListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderItemListViewModel), typeof(OrderOrderItems), new PropertyMetadata(null));
        #endregion

        public ICommand NewCommand => new RelayCommand(New);
        private void New()
        {
            // TODO: 
            //await ViewManager.Current.CreateNewView(typeof(OrderItemView), new OrderItemViewState());
        }
    }
}
