using System;
using System.Collections.Generic;

using Windows.UI.Xaml.Data;

namespace VanArsdel.Inventory.Providers
{
    partial class VirtualCollection<T> : ISelectionInfo
    {
        // List of ranges that form the selection
        private ItemIndexRangeList _selection = new ItemIndexRangeList();

        public void SelectRange(ItemIndexRange itemIndexRange)
        {
            _selection.Add(itemIndexRange);
        }

        public void DeselectRange(ItemIndexRange itemIndexRange)
        {
            _selection.Subtract(itemIndexRange);
        }

        public bool IsSelected(int index)
        {
            foreach (ItemIndexRange range in _selection)
            {
                if (index >= range.FirstIndex && index <= range.LastIndex) return true;
            }
            return false;
        }

        public IReadOnlyList<ItemIndexRange> GetSelectedRanges()
        {
            return _selection.ToList();
        }
    }
}
