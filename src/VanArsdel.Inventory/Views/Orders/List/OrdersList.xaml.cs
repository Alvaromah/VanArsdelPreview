using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

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

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OrdersList;
            control.UpdateViewModel(e.OldValue as OrderListViewModel, e.NewValue as OrderListViewModel);
        }

        private void UpdateViewModel(OrderListViewModel oldViewModel, OrderListViewModel newViewModel)
        {
            if (oldViewModel != null)
            {
                oldViewModel.UpdateView -= OnUpdateView;
            }
            newViewModel.UpdateView += OnUpdateView;
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(OrderListViewModel), typeof(OrdersList), new PropertyMetadata(null, ViewModelChanged));
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
                    //NavigationService.Main.Navigate(typeof(OrderView), new OrderViewState());
                    break;
                case ToolbarButton.Refresh:
                    await ViewModel.RefreshAsync();
                    break;
            }
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            await ViewManager.Current.CreateNewView(typeof(OrdersView));
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
