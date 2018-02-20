using System;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class ShellView : Page
    {
        static public ShellView Current { get; private set; }

        public ShellView()
        {
            Current = this;
            ViewModel = new ShellViewModel();
            InitializeComponent();
            NavigationService.Shell.RegisterFrame(frame);
            NavigationService.Shell.Navigated += OnNavigated;
        }

        #region IsPaneOpen
        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(ShellView), new PropertyMetadata(true));
        #endregion

        public ShellViewModel ViewModel { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.NavigateTo(KnownNavigationItems.Dashboard);
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationItem item)
            {
                ViewModel.NavigateTo(item);
            }
            else if (args.IsSettingsSelected)
            {
                ViewModel.NavigateTo(KnownNavigationItems.Settings);
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            switch (e.SourcePageType.Name)
            {
                case nameof(SettingsView):
                    ViewModel.SelectedItem = navigationView.SettingsItem;
                    break;
                case nameof(CommonView):
                    ViewModel.SelectedItem = ViewModel.Items.Where(r => r.Label == e.Parameter as String).FirstOrDefault();
                    break;
                default:
                    ViewModel.SelectedItem = ViewModel.Items.Where(r => r.Page == e.SourcePageType).FirstOrDefault();
                    break;
            }
        }
    }
}
