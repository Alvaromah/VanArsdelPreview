using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace VanArsdel.Inventory.Providers
{
    abstract public partial class VirtualCollection<T> : IItemsRangeInfo, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public readonly int RangeSize;

        private DispatcherTimer _timer = null;

        public VirtualCollection(int rangeSize = 8)
        {
            RangeSize = rangeSize;
            Ranges = new Dictionary<int, IList<T>>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += OnTimerTick;
        }

        public Dictionary<int, IList<T>> Ranges { get; }

        private bool _isBusy = false;
        private bool _cancel = false;

        private ItemIndexRange _visibleRange = null;
        private IReadOnlyList<ItemIndexRange> _trackedItems = null;

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            _timer.Stop();
            _visibleRange = visibleRange;
            _trackedItems = trackedItems;
            _timer.Start();
        }

        private object _sync = new Object();

        private async void OnTimerTick(object sender, object e)
        {
            _timer.Stop();
            lock (_sync)
            {
                if (_isBusy)
                {
                    _cancel = true;
                    _timer.Start();
                    return;
                }
                _cancel = false;
                _isBusy = true;
            }

            await FetchRange(_visibleRange);
            await FetchRanges(_trackedItems);

            lock (_sync)
            {
                _isBusy = false;
            }
        }

        private async Task FetchRanges(IReadOnlyList<ItemIndexRange> trackedItems)
        {
            foreach (var trackedRange in trackedItems)
            {
                await FetchRange(trackedRange);
                if (_cancel) return;
            }
        }

        private async Task FetchRange(ItemIndexRange trackedRange)
        {
            int firstIndex = trackedRange.FirstIndex / RangeSize;
            int lastIndex = trackedRange.LastIndex / RangeSize;
            for (int index = firstIndex; index <= lastIndex; index++)
            {
                if (!Ranges.ContainsKey(index))
                {
                    System.Diagnostics.Debug.WriteLine("Fetch {0}", index);
                    var items = await FetchDataAsync(index, RangeSize);
                    Ranges[index] = items;
                    for (int n = 0; n < items.Count; n++)
                    {
                        int startIndex = Math.Min(index * RangeSize + n, Count);
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, items[n], DefaultItem, startIndex));
                    }
                }
            }
        }

        #region Dispose
        public void Dispose()
        {
            // TODOX: 
        }
        #endregion

        abstract protected T DefaultItem { get; }

        abstract protected Task<IList<T>> FetchDataAsync(int pageIndex, int pageSize);
    }
}
