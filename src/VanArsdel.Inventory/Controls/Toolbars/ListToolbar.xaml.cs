﻿using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace VanArsdel.Inventory.Controls
{
    public sealed partial class ListToolbar : UserControl
    {
        public event ToolbarButtonClickEventHandler ButtonClick;
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted;

        public ListToolbar()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            UpdateControl();
        }

        #region ToolbarMode
        public ListToolbarMode ToolbarMode
        {
            get { return (ListToolbarMode)GetValue(ToolbarModeProperty); }
            set { SetValue(ToolbarModeProperty, value); }
        }

        private static void ToolbarModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListToolbar;
            control.UpdateControl();
        }

        public static readonly DependencyProperty ToolbarModeProperty = DependencyProperty.Register("ToolbarMode", typeof(ListToolbarMode), typeof(ListToolbar), new PropertyMetadata(ListToolbarMode.Default, ToolbarModeChanged));
        #endregion

        #region Query
        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register("Query", typeof(string), typeof(ListToolbar), new PropertyMetadata(null));
        #endregion

        private void UpdateControl()
        {
            switch (ToolbarMode)
            {
                default:
                case ListToolbarMode.Default:
                    ShowCategory("default");
                    break;
                case ListToolbarMode.Cancel:
                    ShowCategory("cancel");
                    break;
                case ListToolbarMode.CancelDelete:
                    ShowCategory("cancel", "delete");
                    break;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ElementSet.Children<AppBarButton>(commandBar.PrimaryCommands).Click += OnButtonClick;
            ElementSet.Children<AppBarButton>(commandBar.Content).Click += OnButtonClick;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ElementSet.Children<AppBarButton>(commandBar.PrimaryCommands).Click -= OnButtonClick;
            ElementSet.Children<AppBarButton>(commandBar.Content).Click -= OnButtonClick;
        }

        private void ShowCategory(params string[] categories)
        {
            ElementSet.Children<AppBarButton>(commandBar.PrimaryCommands)
                .ForEach(v => v.Show(v.IsCategory(categories)));
            ElementSet.Children<AppBarButton>(commandBar.Content)
                .ForEach(v => v.Show(v.IsCategory(categories)));
            search.Show(search.IsCategory(categories));
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is AppBarButton button)
            {
                switch (button.Name)
                {
                    case "buttonNew":
                        RaiseButtonClick(ToolbarButton.New);
                        break;
                    case "buttonEdit":
                        RaiseButtonClick(ToolbarButton.Edit);
                        break;
                    case "buttonDelete":
                        RaiseButtonClick(ToolbarButton.Delete);
                        break;
                    case "buttonCancel":
                        RaiseButtonClick(ToolbarButton.Cancel);
                        break;
                    case "buttonSelect":
                        RaiseButtonClick(ToolbarButton.Select);
                        break;
                    case "buttonRefresh":
                        RaiseButtonClick(ToolbarButton.Refresh);
                        break;
                }
            }
        }

        private void RaiseButtonClick(ToolbarButton toolbarButton)
        {
            ButtonClick?.Invoke(this, new ToolbarButtonClickEventArgs(toolbarButton));
        }

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            QuerySubmitted?.Invoke(sender, args);
        }
    }
}
