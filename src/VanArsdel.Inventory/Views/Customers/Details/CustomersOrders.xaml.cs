using System;

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

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomersOrders;
            control.UpdateViewModel(e.OldValue as OrderListViewModel, e.NewValue as OrderListViewModel);
        }

        private void UpdateViewModel(OrderListViewModel oldViewModel, OrderListViewModel newViewModel)
        {
            if (oldViewModel != null)
            {
                oldViewModel.UpdateView -= OnUpdateView;
            }
            if (newViewModel != null)
            {
                newViewModel.UpdateView += OnUpdateView;
            }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(OrderListViewModel), typeof(CustomersOrders), new PropertyMetadata(null, ViewModelChanged));
        #endregion

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
