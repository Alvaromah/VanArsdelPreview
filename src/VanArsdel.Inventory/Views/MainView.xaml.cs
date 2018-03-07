using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.Providers;
using VanArsdel.Inventory.ViewModels;

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

        #region StatusMessage
        public string StatusMessage
        {
            get { return (string)GetValue(StatusMessageProperty); }
            set { SetValue(StatusMessageProperty, value); }
        }

        public static readonly DependencyProperty StatusMessageProperty = DependencyProperty.Register("StatusMessage", typeof(string), typeof(MainView), new PropertyMetadata(null));
        #endregion

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Main.RegisterFrame(frame);

            IsBusy = true;
            StatusMessage = "Loading...";
            await Task.Delay(100);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await DataHelper.Current.InitializeAsync(new DataProviderFactory());
            if (e.Parameter is MainViewState state)
            {
                frame.Navigate(state.PageType, state.Parameter);
            }
            stopwatch.Stop();

            IsBusy = false;
            StatusMessage = $"Ready ({stopwatch.Elapsed.TotalMilliseconds.ToString("#,## 'ms'")})";
        }
    }

    public class MainViewState : ViewStateBase
    {
        public Type PageType { get; set; }
        public object Parameter { get; set; }
        public int ViewId { get; set; }
    }
}
