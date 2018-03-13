﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VanArsdel.Inventory.ViewModels
{
    partial class DetailsViewModel<TModel>
    {
        public ICommand BackCommand => new RelayCommand(Back);
        private void Back()
        {
            if (NavigationService.Main.CanGoBack)
            {
                NavigationService.Main.GoBack();
            }
        }

        public ICommand EditCommand => new RelayCommand(Edit);
        virtual protected void Edit()
        {
            BeginEdit();
        }

        public ICommand CancelCommand => new RelayCommand(Cancel);
        virtual protected async void Cancel()
        {
            CancelEdit();
            if (IsNewItem)
            {
                await TryExit();
            }
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
            if (await ConfirmDeleteAsync())
            {
                await DeletetAsync();
            }
        }

        abstract protected Task<bool> ConfirmDeleteAsync();
    }
}
