using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersDetails : UserControl
    {
        public event EventHandler ItemDeleted;

        public CustomersDetails()
        {
            InitializeComponent();
        }

        #region ViewModel
        public CustomerDetailsViewModel ViewModel
        {
            get { return (CustomerDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomersDetails;
            control.UpdateViewModel(e.OldValue as CustomerDetailsViewModel, e.NewValue as CustomerDetailsViewModel);
        }

        private void UpdateViewModel(CustomerDetailsViewModel oldViewModel, CustomerDetailsViewModel newViewModel)
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

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerDetailsViewModel), typeof(CustomersDetails), new PropertyMetadata(null, ViewModelChanged));
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
                    if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current customer?", "Ok", "Cancel"))
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
