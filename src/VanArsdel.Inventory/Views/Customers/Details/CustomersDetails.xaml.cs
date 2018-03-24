using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomersDetails : UserControl
    {
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

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CustomerDetailsViewModel), typeof(CustomersDetails), new PropertyMetadata(null));
        #endregion
    }
}
