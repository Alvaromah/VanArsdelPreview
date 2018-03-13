using System;
using System.Windows.Input;

namespace VanArsdel.Inventory.ViewModels
{
    partial class DetailsViewModel<TModel>
    {
        public ICommand EditCommand => new RelayCommand(Edit);
        virtual protected void Edit()
        {
            BeginEdit();
        }

        public ICommand CancelCommand => new RelayCommand(Cancel);
        virtual protected void Cancel()
        {
            CancelEdit();
        }

        public ICommand SaveCommand => new RelayCommand(Save);
        virtual protected async void Save()
        {
            var result = Validate();
            if (result.IsOk)
            {
                await SaveAsync();
            }
            else
            {
                await DialogBox.ShowAsync(result);
            }
        }

        public ICommand DeleteCommand => new RelayCommand(Delete);
        virtual protected async void Delete()
        {
            if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current customer?", "Ok", "Cancel"))
            {
                await DeletetAsync();
            }
        }
    }
}
