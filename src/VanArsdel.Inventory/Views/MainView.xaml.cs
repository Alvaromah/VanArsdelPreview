using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            InitializeComponent();
        }

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(MainView), new PropertyMetadata(false));
        #endregion

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            IsBusy = true;
            await Task.Delay(100);
            await DataHelper.Current.InitializeAsync(new DataProviderFactory());
            if (e.Parameter is MainViewState state)
            {
                frame.Navigate(state.PageType, state.Parameter);
            }
            IsBusy = false;
        }
    }

    public class MainViewState
    {
        public Type PageType { get; set; }
        public object Parameter { get; set; }
    }
}
