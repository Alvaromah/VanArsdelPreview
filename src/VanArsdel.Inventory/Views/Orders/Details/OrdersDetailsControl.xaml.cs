using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Controls;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrdersDetailsControl : UserControl
    {
        public event RoutedEventHandler InputGotFocus;

        public OrdersDetailsControl()
        {
            InitializeComponent();
            InitializeInputs();
        }

        #region Model
        public OrderModel Model
        {
            get { return (OrderModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(OrderModel), typeof(OrdersDetailsControl), new PropertyMetadata(null));
        #endregion

        #region IsEditMode
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        private static void IsEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OrdersDetailsControl;
            control.UpdateEditMode();
        }

        public static readonly DependencyProperty IsEditModeProperty = DependencyProperty.Register("IsEditMode", typeof(bool), typeof(OrdersDetailsControl), new PropertyMetadata(false, IsEditModeChanged));
        #endregion

        private void InitializeInputs()
        {
            ElementSet.Children<LabelTextBox>(this).GotFocus += OnInputGotFocus;
            ElementSet.Children<LabelComboBox>(this).GotFocus += OnInputGotFocus;
        }

        private void OnInputGotFocus(object sender, RoutedEventArgs e)
        {
            InputGotFocus?.Invoke(sender, e);
        }

        private void UpdateEditMode()
        {
            ElementSet.Children<LabelTextBox>(this).ForEach(c => c.Mode = IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
            ElementSet.Children<LabelComboBox>(this).ForEach(c => c.Mode = IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
        }
    }
}
