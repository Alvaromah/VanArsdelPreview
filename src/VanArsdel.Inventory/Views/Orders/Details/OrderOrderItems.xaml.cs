﻿using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrderOrderItems : UserControl
    {
        public OrderOrderItems()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderItemListViewModel ViewModel
        {
            get { return (OrderItemListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderItemListViewModel), typeof(OrderOrderItems), new PropertyMetadata(null));
        #endregion
    }
}
