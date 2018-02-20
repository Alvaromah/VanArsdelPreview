using System;
using System.Collections.Generic;

namespace VanArsdel.Inventory.Data
{
    public class PageResult<T>
    {
        public PageResult(int pageIndex, int pageSize, int count, IList<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Items = items;
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int Count { get; }

        public IList<T> Items { get; }
    }
}
