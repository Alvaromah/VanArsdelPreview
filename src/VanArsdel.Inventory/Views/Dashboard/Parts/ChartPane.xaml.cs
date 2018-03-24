using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class ChartPane : UserControl
    {
        public ChartPane()
        {
            InitializeComponent();
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
