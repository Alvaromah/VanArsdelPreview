using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory.Controls
{
    public sealed partial class DataList : UserControl
    {
        public DataList()
        {
            InitializeComponent();
        }

        #region ItemsSource
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataList;
            control.Bindings.Update();
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DataList), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(DataList), new PropertyMetadata(null));
        #endregion


        #region SelectedItem
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region IsMultipleSelection
        public bool IsMultipleSelection
        {
            get { return (bool)GetValue(IsMultipleSelectionProperty); }
            set { SetValue(IsMultipleSelectionProperty, value); }
        }

        public static readonly DependencyProperty IsMultipleSelectionProperty = DependencyProperty.Register("IsMultipleSelection", typeof(bool), typeof(DataList), new PropertyMetadata(false));
        #endregion

        #region AddedSelectedItemsCommand
        public ICommand AddedSelectedItemsCommand
        {
            get { return (ICommand)GetValue(AddedSelectedItemsCommandProperty); }
            set { SetValue(AddedSelectedItemsCommandProperty, value); }
        }

        public static readonly DependencyProperty AddedSelectedItemsCommandProperty = DependencyProperty.Register("AddedSelectedItemsCommand", typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region RemovedSelectedItemsCommand
        public ICommand RemovedSelectedItemsCommand
        {
            get { return (ICommand)GetValue(RemovedSelectedItemsCommandProperty); }
            set { SetValue(RemovedSelectedItemsCommandProperty, value); }
        }

        public static readonly DependencyProperty RemovedSelectedItemsCommandProperty = DependencyProperty.Register("RemovedSelectedItemsCommand", typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion


        #region ToolbarMode
        public ListToolbarMode ToolbarMode
        {
            get { return (ListToolbarMode)GetValue(ToolbarModeProperty); }
            set { SetValue(ToolbarModeProperty, value); }
        }

        public static readonly DependencyProperty ToolbarModeProperty = DependencyProperty.Register("ToolbarMode", typeof(ListToolbarMode), typeof(DataList), new PropertyMetadata(ListToolbarMode.Default));
        #endregion

        #region Query
        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register("Query", typeof(string), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region IsDisabled
        public bool IsDisabled
        {
            get { return (bool)GetValue(IsDisabledProperty); }
            set { SetValue(IsDisabledProperty, value); }
        }

        public static readonly DependencyProperty IsDisabledProperty = DependencyProperty.Register(nameof(IsDisabled), typeof(bool), typeof(DataList), new PropertyMetadata(false));
        #endregion

        #region SelectionMode
        public ListViewSelectionMode SelectionMode
        {
            get { return (ListViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(ListViewSelectionMode), typeof(DataList), new PropertyMetadata(ListViewSelectionMode.Single));
        #endregion


        #region QuerySubmittedCommand
        public ICommand QuerySubmittedCommand
        {
            get { return (ICommand)GetValue(QuerySubmittedCommandProperty); }
            set { SetValue(QuerySubmittedCommandProperty, value); }
        }

        public static readonly DependencyProperty QuerySubmittedCommandProperty = DependencyProperty.Register(nameof(QuerySubmittedCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region NewCommand
        public ICommand NewCommand
        {
            get { return (ICommand)GetValue(NewCommandProperty); }
            set { SetValue(NewCommandProperty, value); }
        }

        public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register("NewCommand", typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region RefreshCommand
        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region StartSelectionCommand
        public ICommand StartSelectionCommand
        {
            get { return (ICommand)GetValue(StartSelectionCommandProperty); }
            set { SetValue(StartSelectionCommandProperty, value); }
        }

        public static readonly DependencyProperty StartSelectionCommandProperty = DependencyProperty.Register(nameof(StartSelectionCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region CancelSelectionCommand
        public ICommand CancelSelectionCommand
        {
            get { return (ICommand)GetValue(CancelSelectionCommandProperty); }
            set { SetValue(CancelSelectionCommandProperty, value); }
        }

        public static readonly DependencyProperty CancelSelectionCommandProperty = DependencyProperty.Register(nameof(CancelSelectionCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        public bool IsDataAvailable => (ItemsSource?.Cast<object>().Any() ?? false);
        public bool IsDataUnavailable => !IsDataAvailable;
        public string DataUnavailableMessage => ItemsSource == null ? "Loading..." : "No items found.";

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsMultipleSelection)
            {
                ToolbarMode = listview.SelectedItems.Any() ? ListToolbarMode.CancelDelete : ListToolbarMode.Cancel;
            }
            if (AddedSelectedItemsCommand?.CanExecute(e.AddedItems) ?? false)
            {
                AddedSelectedItemsCommand.Execute(e.AddedItems);
            }
            if (RemovedSelectedItemsCommand?.CanExecute(e.RemovedItems) ?? false)
            {
                RemovedSelectedItemsCommand.Execute(e.RemovedItems);
            }
        }

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.New:
                    if (NewCommand?.CanExecute(null) ?? false)
                    {
                        NewCommand.Execute(null);
                    }
                    break;
                case ToolbarButton.Delete:
                    if (DeleteCommand?.CanExecute(null) ?? false)
                    {
                        DeleteCommand.Execute(null);
                    }
                    break;
                case ToolbarButton.Select:
                    ToolbarMode = ListToolbarMode.Cancel;
                    break;
                case ToolbarButton.Refresh:
                    if (RefreshCommand?.CanExecute(null) ?? false)
                    {
                        RefreshCommand.Execute(null);
                    }
                    break;
                case ToolbarButton.Cancel:
                    listview.SelectedItems.Clear();
                    ToolbarMode = ListToolbarMode.Default;
                    SelectedItem = ItemsSource?.Cast<object>().FirstOrDefault();
                    break;
            }
        }
    }
}
