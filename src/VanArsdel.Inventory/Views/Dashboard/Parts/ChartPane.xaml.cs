using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Threading.Tasks;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class ChartPane : UserControl
    {
        WebView _webView;
        public ChartPane()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;

            //Initialze the WebView in a separate thread and Add it to the grid
            _webView = new WebView(WebViewExecutionMode.SeparateThread);
            _webView.SetValue(Grid.RowProperty, 1);
            _webView.NavigationCompleted += OnNavigationCompleted;

        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //string js = "var dataSet = [12, 19, 3, 5];";
            //string result = await _webView.InvokeScriptAsync("eval", new string[] { js});
            RootGridLayout.Children.Add(_webView);
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            string text = await LoadStringFromPackageFileAsync("ChartHtmlControl.html");
            _webView.NavigateToString(text);
        }

        public static async Task<string> LoadStringFromPackageFileAsync(string name)
        {
            // Using the storage classes to read the content from a file as a string.
            StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///html/{name}"));
            return await FileIO.ReadTextAsync(f);
        }


        public IEnumerable<SerieValue> Series => GetSeries();

        private IEnumerable<SerieValue> GetSeries()
        {
            yield return new SerieValue { Value = 40, IsSelected = true };
            yield return new SerieValue { Value = 15 };
            yield return new SerieValue { Value = 20 };
            yield return new SerieValue { Value = 25 };
        }
    }

    public class SerieValue
    {
        public double Value { get; set; }
        public bool IsSelected { get; set; }
    }
}
