using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using VanArsdel.Inventory.Controls;

namespace VanArsdel.Inventory.ViewModels
{
    partial class ListViewModel<TModel>
    {
        public string Query { get; set; }

        private ListToolbarMode _toolbarMode = ListToolbarMode.Default;
        public ListToolbarMode ToolbarMode
        {
            get => _toolbarMode;
            set => Set(ref _toolbarMode, value);
        }

        private IList<TModel> _items = null;
        public IList<TModel> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        private bool _isMultipleSelection = false;
        public bool IsMultipleSelection
        {
            get => _isMultipleSelection;
            set => Set(ref _isMultipleSelection, value);
        }

        private TModel _selectedItem = default(TModel);
        public TModel SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private int _itemsCount = 0;
        public int ItemsCount
        {
            get => _itemsCount;
            set => Set(ref _itemsCount, value);
        }

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => _pageIndex;
            set { if (Set(ref _pageIndex, value)) Refresh(); }
        }

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => Set(ref _pageSize, value);
        }

        public override string Title => String.IsNullOrEmpty(Query) ? " " : $"results for \"{Query}\"";

        public string DataUnavailableMessage => Items == null ? "Loading..." : "No items found";

        public bool IsDataAvailable => (_items?.Count ?? 0) > 0;
        public bool IsDataUnavailable => !IsDataAvailable;
    }
}
