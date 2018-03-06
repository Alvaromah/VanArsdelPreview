using System;

using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace VanArsdel.Inventory
{
    public class NavigationService
    {
        public event NavigatingCancelEventHandler Navigating;
        public event NavigatedEventHandler Navigated;
        public event NavigationStoppedEventHandler NavigationStopped;
        public event NavigationFailedEventHandler NavigationFailed;

        static public NavigationService Main => ThreadSafeSingleton<NavigationService>.Instance;
        static public NavigationService Shell { get; private set; }

        static NavigationService()
        {
            Shell = new NavigationService();
        }

        private SystemNavigationManager CurrentView => SystemNavigationManager.GetForCurrentView();

        public Frame Frame { get; private set; }

        public bool CanGoBack => Frame.CanGoBack;

        public bool CanGoForward => Frame.CanGoForward;

        public void GoBack() => Frame.GoBack();

        public void GoForward() => Frame.GoForward();

        public void RegisterFrame(Frame frame)
        {
            Frame = frame;
            if (Frame != null)
            {
                Frame.Navigating -= OnNavigating;
                Frame.Navigated -= OnNavigated;
                Frame.NavigationStopped -= OnNavigationStopped;
                Frame.NavigationFailed -= OnNavigationFailed;
            }
            Frame.Navigating += OnNavigating;
            Frame.Navigated += OnNavigated;
            Frame.NavigationStopped += OnNavigationStopped;
            Frame.NavigationFailed += OnNavigationFailed;

            if (CurrentView != null)
            {
                CurrentView.BackRequested += OnBackRequested;
            }
        }

        public string GetNavigationState()
        {
            return Frame.GetNavigationState();
        }

        public void SetNavigationState(string navigationState)
        {
            Frame.SetNavigationState(navigationState);
            CurrentView.AppViewBackButtonVisibility = CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        public void HideBackButton()
        {
            CurrentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public bool Navigate(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            if (Frame == null)
            {
                throw new InvalidOperationException("Navigation frame not initialized.");
            }
            return Frame.Navigate(pageType, parameter, infoOverride);
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            Navigating?.Invoke(sender, e);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            CurrentView.AppViewBackButtonVisibility = CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            Navigated?.Invoke(sender, e);
        }

        private void OnNavigationStopped(object sender, NavigationEventArgs e)
        {
            NavigationStopped?.Invoke(sender, e);
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (CanGoBack)
            {
                GoBack();
                e.Handled = true;
            }
        }
    }
}
