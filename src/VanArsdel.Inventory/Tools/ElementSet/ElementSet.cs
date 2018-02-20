﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VanArsdel.Inventory
{
    public partial class ElementSet : ElementSet<FrameworkElement>
    {
    }

    public partial class ElementSet<T> : IEnumerable<T> where T : UIElement
    {
        public ElementSet()
        {
        }
        public ElementSet(IEnumerable<T> enumerable)
        {
            Enumerable = enumerable;
        }

        public IEnumerable<T> Enumerable { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerable.GetEnumerator();
        }

        public ElementSet<T> ForEach(Action<T> action)
        {
            foreach (var item in Enumerable)
            {
                action(item);
            }
            return this;
        }

        public ElementSet<T> ForEach<S>(Action<S> action) where S : UIElement
        {
            foreach (var item in Enumerable)
            {
                var target = item as S;
                if (target != null)
                {
                    action(target);
                }
            }
            return this;
        }

        public ElementSet<T> Reverse()
        {
            return new ElementSet<T>(Enumerable.Reverse());
        }

        static public ElementSet<S> Children<S>(DependencyObject source, string category) where S : FrameworkElement
        {
            return new ElementSet<S>(GetChildren<S>(source, v => v.IsCategory(category)));
        }

        static public ElementSet<S> Children<S>(object source, Func<S, bool> predicate = null) where S : UIElement
        {
            return new ElementSet<S>(GetChildren<S>(source, predicate));
        }
        static public ElementSet<S> Children<S>(DependencyObject source, Func<S, bool> predicate = null) where S : UIElement
        {
            return new ElementSet<S>(GetChildren<S>(source, predicate));
        }

        static private IEnumerable<S> GetChildren<S>(object source, Func<S, bool> predicate = null) where S : UIElement
        {
            if (source is UIElement element)
            {
                foreach (var item in GetChildren<S>(element, predicate))
                {
                    yield return item;
                }
                yield break;
            }

            predicate = predicate ?? new Func<S, bool>((e) => true);
            if (source is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item is S match)
                    {
                        if (predicate(match))
                        {
                            yield return match;
                        }
                    }
                    if (item is DependencyObject depObject)
                    {
                        foreach (var elem in GetChildren<S>(depObject, predicate))
                        {
                            yield return elem;
                        }
                    }
                }
            }
        }

        static private IEnumerable<S> GetChildren<S>(DependencyObject source, Func<S, bool> predicate = null) where S : UIElement
        {
            predicate = predicate ?? new Func<S, bool>((e) => true);
            if (source != null)
            {
                var count = VisualTreeHelper.GetChildrenCount(source);
                for (int n = 0; n < count; n++)
                {
                    var child = VisualTreeHelper.GetChild(source, n);
                    if (child is S match)
                    {
                        if (predicate(match))
                        {
                            yield return match;
                        }
                    }
                }
                for (int n = 0; n < count; n++)
                {
                    var child = VisualTreeHelper.GetChild(source, n);
                    foreach (var item in GetChildren<S>(child, predicate))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
