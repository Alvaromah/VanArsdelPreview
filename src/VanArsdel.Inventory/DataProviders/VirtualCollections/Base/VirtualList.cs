using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace VanArsdel.Inventory
{
    abstract public class VirtualList<T> : IList, IList<T>, IItemsRangeInfo, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected readonly int RangeSize;

        protected Dictionary<int, IList<T>> _ranges = null;

        private DispatcherTimer _timer = null;

        public VirtualList(int rangeSize = 32)
        {
            RangeSize = rangeSize;
            _ranges = new Dictionary<int, IList<T>>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += OnTimerTick;
        }

        public bool IsFixedSize => false;
        public bool IsReadOnly => false;

        object IList.this[int index]
        {
            get => GetItem(index);
            set => throw new NotImplementedException();
        }

        public T this[int index]
        {
            get => GetItem(index);
            set { throw new NotImplementedException(); }
        }

        virtual protected T GetItem(int index)
        {
            int rangeIndex = index / RangeSize;
            if (_ranges.ContainsKey(rangeIndex))
            {
                var range = _ranges[rangeIndex];
                if (range != null)
                {
                    return range[index % RangeSize];
                }
            }
            return DefaultItem;
        }

        private ItemIndexRange _visibleRange = null;
        private IReadOnlyList<ItemIndexRange> _trackedItems = null;

        private bool _isBusy = false;
        private bool _cancel = false;

        private object _sync = new Object();

        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            _timer.Stop();
            _visibleRange = visibleRange;
            _trackedItems = trackedItems;
            _timer.Start();
        }

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
            }
        }

        private async Task FetchRange(ItemIndexRange trackedRange)
        {
            int firstIndex = trackedRange.FirstIndex / RangeSize;
            int lastIndex = trackedRange.LastIndex / RangeSize;
            for (int index = firstIndex; index <= lastIndex; index++)
            {
                if (_cancel) return;

                if (!_ranges.ContainsKey(index))
                {
                    var items = await FetchDataAsync(index, RangeSize);
                    _ranges[index] = items;
                    for (int n = 0; n < items.Count; n++)
                    {
                        int startIndex = Math.Min(index * RangeSize + n, Count);
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, items[n], DefaultItem, startIndex));
                    }
                }
            }
        }

        #region IList Not Implemented
        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }


        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            return 0;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            // TODOX: 
        }
        #endregion

        abstract public int Count { get; }
        abstract protected T DefaultItem { get; }

        abstract protected Task<IList<T>> FetchDataAsync(int pageIndex, int pageSize);

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetItems().GetEnumerator();
        }

        private IEnumerable<T> GetItems()
        {
            for (int n = 0; n < Count; n++)
            {
                yield return this[n];
            }
        }
    }
}
