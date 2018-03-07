using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

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

        private async void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await ViewModel.RefreshAsync(resetPageIndex: true);
        }

        private async void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.New:
                    // TODO: Open Order instead
                    //NavigationService.Main.Navigate(typeof(CustomerView), new CustomerViewState());
                    break;
                case ToolbarButton.Refresh:
                    await ViewModel.RefreshAsync();
                    break;
            }
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
