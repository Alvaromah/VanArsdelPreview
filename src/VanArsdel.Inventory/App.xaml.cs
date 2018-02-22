using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Storage;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.Views;

namespace VanArsdel.Inventory
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeDatabase();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 840);
        }

        public Type EntryPage => typeof(ShellView);

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;

            if (frame == null)
            {
                frame = new Frame();
                Window.Current.Content = frame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (frame.Content == null)
                {
                    var state = new MainViewState
                    {
                        PageType = EntryPage,
                        Parameter = e.Arguments
                    };
                    frame.Navigate(typeof(MainView), state);
                }
                Window.Current.Activate();
            }

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private async void InitializeDatabase()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync("VanArsdel.db") == null)
            {
                var sourceDatabaseFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Database/VanArsdel.db"));
                var targetDatabaseFile = await folder.CreateFileAsync("VanArsdel.db", CreationCollisionOption.ReplaceExisting);
                await sourceDatabaseFile.CopyAndReplaceAsync(targetDatabaseFile);
            }
        }
    }
}
