using System;
using System.Windows.Input;

namespace VanArsdel.Inventory.ViewModels
{
    partial class DetailsViewModel<TModel>
    {
        public ICommand EditCommand => new RelayCommand(Edit);
        private void Edit()
        {
            BeginEdit();
        }

        public ICommand CancelCommand => new RelayCommand(Cancel);
        private void Cancel()
        {
            CancelEdit();
        }

        public ICommand SaveCommand => new RelayCommand(Save);
        private async void Save()
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
        private async void Delete()
        {
            if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete selected customer?", "Ok", "Cancel"))
            {
                await DeletetAsync();
            }
        }
    }
}
