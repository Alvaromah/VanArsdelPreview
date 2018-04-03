﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace VanArsdel.Inventory.Providers
{
    partial class VirtualCollection<T> : IList, IList<T> where T : class
    {
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;

        public int Count { get; set; }

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
            if (Ranges.ContainsKey(rangeIndex))
            {
                var range = Ranges[rangeIndex];
                if (range != null)
                {
                    return range[index % RangeSize];
                }
            }
            return DefaultItem;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetItems().GetEnumerator();
        }

        private IEnumerable<T> GetItems()
        {
            for (int n = 0; n < Count; n++)
            {
                System.Diagnostics.Debug.WriteLine(n);
                yield return this[n];
            }
        }

        public int IndexOf(object value)
        {
            return IndexOf(value as T);
        }

        public int IndexOf(T item)
        {
            if (item != null)
            {
                foreach (var range in Ranges)
                {
                    int index = range.Value.IndexOf(item);
                    if (index > -1)
                    {
                        return range.Key * RangeSize + index;
                    }
                }
            }
            return -1;
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
        #endregion

        #region IList<T> Not Implemented
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
        #endregion
    }
}
