﻿using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.Data;
using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class DashboardView : Page
    {
        public DashboardView()
        {
            InitializeViewModel();
            InitializeComponent();
        }

        public DashboardViewModel ViewModel { get; private set; }

        private void InitializeViewModel()
        {
            ViewModel = new DashboardViewModel(new DataProviderFactory());
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await Task.Delay(100);
            await ViewModel.LoadAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unload();
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Control control)
            {
                ViewModel.ItemSelected(control.Tag as String);
            }
        }
    }
}
