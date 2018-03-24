﻿using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory.Services
{
    public interface INavigationService
    {
        bool IsMainView { get; }

        bool CanGoBack { get; }

        void Initialize(Frame frame);

        bool Navigate<TViewModel>(object parameter = null);
        bool Navigate(Type viewModelType, object parameter = null);

        Task<int> CreateNewViewAsync<TViewModel>(object parameter = null);
        Task<int> CreateNewViewAsync(Type viewModelType, object parameter = null);

        void GoBack();

        Task CloseViewAsync();
    }
}
