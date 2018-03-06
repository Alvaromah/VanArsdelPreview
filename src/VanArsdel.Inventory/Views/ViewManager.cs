using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;

namespace VanArsdel.Inventory.Views
{
    public class ViewManager
    {
        static public ViewManager Current { get; }

        static ViewManager()
        {
            Current = new ViewManager();
        }

        private ViewManager()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
        }

        public int MainViewId { get; }

        public async Task<int> CreateNewView(Type pageType, object parameter = null)
        {
            int viewId = 0;

            CoreApplicationView newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewId = ApplicationView.GetForCurrentView().Id;

                var state = new MainViewState
                {
                    PageType = pageType,
                    Parameter = parameter,
                    ViewId = viewId
                };

                var frame = new Frame();
                frame.Navigate(typeof(MainView), state);

                Window.Current.Content = frame;
                Window.Current.Activate();
            });

            if (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId))
            {
                return viewId;
            }

            return 0;
        }

        public async Task Close()
        {
            await ApplicationViewSwitcher.SwitchAsync(MainViewId, ApplicationView.GetForCurrentView().Id, ApplicationViewSwitchingOptions.ConsolidateViews);
        }
    }
}
