using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Controls;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomerDetails : UserControl
    {
        public event RoutedEventHandler InputGotFocus;

        public CustomerDetails()
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

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CustomerDetails;
            control.UpdateViewModel(e.OldValue as CustomerDetailsViewModel, e.NewValue as CustomerDetailsViewModel);
        }

        private void UpdateViewModel(CustomerDetailsViewModel oldViewModel, CustomerDetailsViewModel newViewModel)
        {
            if (oldViewModel != null)
            {
                oldViewModel.UpdateView -= OnUpdateView;
            }
            newViewModel.UpdateView += OnUpdateView;
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerDetailsViewModel), typeof(CustomerDetails), new PropertyMetadata(null, ViewModelChanged));
        #endregion

        public void SetFocus()
        {
            ElementSet.Children<LabelTextBox>(this).FirstOrDefault()?.SetFocus();
        }

        private void InitializeInputs()
        {
            ElementSet.Children<LabelTextBox>(this).GotFocus += OnInputGotFocus;
            ElementSet.Children<LabelComboBox>(this).GotFocus += OnInputGotFocus;
        }

        private void OnInputGotFocus(object sender, RoutedEventArgs e)
        {
            InputGotFocus?.Invoke(sender, e);
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            UpdateEditMode();
            Bindings.Update();
        }

        private void UpdateEditMode()
        {
            ElementSet.Children<LabelTextBox>(this).ForEach(c => c.Mode = ViewModel.IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
            ElementSet.Children<LabelComboBox>(this).ForEach(c => c.Mode = ViewModel.IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
        }
    }
}
