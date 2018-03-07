using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

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

        private async void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await ViewModel.RefreshAsync(resetPageIndex: true);
        }

        private async void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.New:
                    NavigationService.Main.Navigate(typeof(CustomerView), new CustomerViewState());
                    break;
                case ToolbarButton.Select:
                    ViewModel.IsMultipleSelection = true;
                    ViewModel.ToolbarMode = ListToolbarMode.Cancel;
                    break;
                case ToolbarButton.Refresh:
                    await ViewModel.RefreshAsync();
                    break;
                case ToolbarButton.Cancel:
                    ViewModel.IsMultipleSelection = false;
                    ViewModel.ToolbarMode = ListToolbarMode.Default;
                    break;
            }
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
