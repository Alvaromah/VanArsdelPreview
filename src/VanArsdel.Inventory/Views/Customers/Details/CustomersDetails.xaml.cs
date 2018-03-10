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
            InitializeInputs();
        }

        #region ViewModel
        public CustomerDetailsViewModel ViewModel
        {
            get { return (CustomerDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerDetailsViewModel), typeof(CustomersDetails), new PropertyMetadata(null));
        #endregion

        #region IsEditMode
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        private static void IsEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomersDetails;
            control.UpdateEditMode();
        }

        public static readonly DependencyProperty IsEditModeProperty = DependencyProperty.Register(nameof(IsEditMode), typeof(bool), typeof(CustomersDetails), new PropertyMetadata(null, IsEditModeChanged));
        #endregion

        private void InitializeInputs()
        {
            ElementSet.Children<LabelTextBox>(this).GotFocus += OnInputGotFocus;
            ElementSet.Children<LabelComboBox>(this).GotFocus += OnInputGotFocus;
        }

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

        private void UpdateEditMode()
        {
            ElementSet.Children<LabelTextBox>(this).ForEach(c => c.Mode = IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
            ElementSet.Children<LabelComboBox>(this).ForEach(c => c.Mode = IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
        }
    }
}
