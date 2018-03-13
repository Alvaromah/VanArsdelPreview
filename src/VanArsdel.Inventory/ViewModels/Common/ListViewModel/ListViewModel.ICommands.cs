using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace VanArsdel.Inventory.ViewModels
{
    partial class ListViewModel<TModel>
    {
        public ICommand RefreshCommand => new RelayCommand(Refresh);
        virtual protected void Refresh()
        {
            Refresh(false);
        }

        public List<TModel> _selectedItems = null;

        public ICommand StartSelectionCommand => new RelayCommand(StartSelection);
        virtual protected void StartSelection()
        {
            _selectedItems = new List<TModel>();
            IsMultipleSelection = true;
        }

        public ICommand CancelSelectionCommand => new RelayCommand(CancelSelection);
        virtual protected void CancelSelection()
        {
            _selectedItems?.Clear();
            _selectedItems = null;
            IsMultipleSelection = false;
            SelectedItem = Items?.FirstOrDefault();
        }

        public ICommand SelectItemsCommand => new RelayCommand<IList<object>>(SelectItems);
        virtual protected void SelectItems(IList<object> items)
        {
            if (IsMultipleSelection)
            {
                foreach (TModel item in items)
                {
                    _selectedItems.Add(item);
                }
            }
        }

        public ICommand DeselectItemsCommand => new RelayCommand<IList<object>>(DeselectItems);
        virtual protected void DeselectItems(IList<object> items)
        {
            if (IsMultipleSelection)
            {
                foreach (TModel item in items)
                {
                    _selectedItems.Remove(item);
                }
            }
        }

        public ICommand DeleteDelectionCommand => new RelayCommand(DeleteDelection);
        virtual protected async void DeleteDelection()
        {
            if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete selected customers?", "Ok", "Cancel"))
            {
                await DeleteSelectionAsync();
                _selectedItems?.Clear();
                _selectedItems = null;
                IsMultipleSelection = false;
                SelectedItem = Items?.FirstOrDefault();
            }
        }
    }
}
