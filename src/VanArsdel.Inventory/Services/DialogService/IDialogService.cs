﻿using System;
using System.Threading.Tasks;

namespace VanArsdel.Inventory.Services
{
    public interface IDialogService
    {
        Task ShowAsync(string title, Exception ex, string ok = "Ok");
        Task ShowAsync(Result result, string ok = "Ok");

        Task<bool> ShowAsync(string title, string content, string ok = "Ok", string cancel = null);
    }
}
