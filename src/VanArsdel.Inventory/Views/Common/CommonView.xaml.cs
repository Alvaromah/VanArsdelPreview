using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CommonView : Page
    {
        public CommonView()
        {
            InitializeComponent();
        }

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CommonView), new PropertyMetadata(null));
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Text = (e.Parameter ?? "").ToString();

            GC.Collect();
        }
    }
}
