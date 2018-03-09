using System;
using System.Collections.Concurrent;
using System.ComponentModel;

using Windows.UI.Xaml;

namespace VanArsdel.Inventory
{
    public class DependencyExpressions
    {
        private ConcurrentDictionary<string, DependencyExpression> _dependencyMap = new ConcurrentDictionary<string, DependencyExpression>();

        public void Initialize(INotifyExpressionChanged source)
        {
            source.PropertyChanged += OnPropertyChanged;
            foreach (var de in _dependencyMap.Values)
            {
                if (de.DependencyProperties != null)
                {
                    foreach (var dp in de.DependencyProperties)
                    {
                        source.RegisterPropertyChangedCallback(dp, (s, d) => { source.RaisePropertyChanged(de.Name); });
                    }
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var source = sender as INotifyExpressionChanged;

            foreach (var de in _dependencyMap.Values)
            {
                if (de.DependencyExpressions != null)
                {
                    foreach (var exp in de.DependencyExpressions)
                    {
                        if (exp.Name == e.PropertyName)
                        {
                            source.RaisePropertyChanged(de.Name);
                        }
                    }
                }
            }
        }

        public DependencyExpression Register(string propertyName, params DependencyProperty[] dps)
        {
            var de = new DependencyExpression(propertyName, dps);
            if (_dependencyMap.TryAdd(propertyName, de))
            {
                return de;
            }
            throw new ArgumentException($"DependencyExpression already registered for property '{propertyName}'.", propertyName);
        }

        public DependencyExpression Register(string propertyName, params DependencyExpression[] dps)
        {
            var de = new DependencyExpression(propertyName, dps);
            if (_dependencyMap.TryAdd(propertyName, de))
            {
                return de;
            }
            throw new ArgumentException($"DependencyExpression already registered for property '{propertyName}'.", propertyName);
        }
    }
}
