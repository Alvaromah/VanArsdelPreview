using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomerDetails : UserControl
    {
        public CustomerDetails()
        {
            InitializeComponent();
        }

        #region Model
        public CustomerModel Model
        {
            get { return (CustomerModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(CustomerModel), typeof(CustomerDetails), new PropertyMetadata(null));
        #endregion
    }
}
