using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrdersDetails : UserControl
    {
        public event EventHandler ItemDeleted;

        public OrdersDetails()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderDetailsViewModel ViewModel
        {
            get { return (OrderDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OrdersDetails;
            control.UpdateViewModel(e.OldValue as OrderDetailsViewModel, e.NewValue as OrderDetailsViewModel);
        }

        private void UpdateViewModel(OrderDetailsViewModel oldViewModel, OrderDetailsViewModel newViewModel)
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

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(OrderDetailsViewModel), typeof(OrdersDetails), new PropertyMetadata(null, ViewModelChanged));
        #endregion

        private async void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.Edit:
                    ViewModel.BeginEdit();
                    break;
                case ToolbarButton.Cancel:
                    this.Focus(FocusState.Programmatic);
                    ViewModel.CancelEdit();
                    break;
                case ToolbarButton.Save:
                    var result = ViewModel.Validate();
                    if (result.IsOk)
                    {
                        this.Focus(FocusState.Programmatic);
                        await ViewModel.SaveAsync();
                    }
                    else
                    {
                        await DialogBox.ShowAsync(result);
                    }
                    break;
                case ToolbarButton.Delete:
                    if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current order?", "Ok", "Cancel"))
                    {
                        await ViewModel.DeletetAsync();
                        ItemDeleted?.Invoke(this, EventArgs.Empty);
                    }
                    break;
            }
        }

        private void OnInputGotFocus(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsEditMode)
            {
                ViewModel.BeginEdit();
            }
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            Bindings.Update();
        }
    }
}
