using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory.Controls
{
    public sealed partial class DataList : UserControl, INotifyExpressionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DataList()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        static private readonly DependencyExpressions DependencyExpressions = new DependencyExpressions();

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DependencyExpressions.Initialize(this);
        }

        private void OnUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DependencyExpressions.Uninitialize(this);
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
            DependencyExpressions.UpdateDependencies(control, nameof(ItemsSource));
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(DataList), new PropertyMetadata(null, ItemsSourceChanged));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(DataList), new PropertyMetadata(null));
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

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region IsMultipleSelection
        public bool IsMultipleSelection
        {
            get { return (bool)GetValue(IsMultipleSelectionProperty); }
            set { SetValue(IsMultipleSelectionProperty, value); }
        }

        private static void IsMultipleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataList;
            DependencyExpressions.UpdateDependencies(control, nameof(IsMultipleSelection));
        }

        public static readonly DependencyProperty IsMultipleSelectionProperty = DependencyProperty.Register(nameof(IsMultipleSelection), typeof(bool), typeof(DataList), new PropertyMetadata(null, IsMultipleSelectionChanged));
        #endregion

        #region SelectedItemsCount
        public int SelectedItemsCount
        {
            get { return (int)GetValue(SelectedItemsCountProperty); }
            set { SetValue(SelectedItemsCountProperty, value); }
        }

        private static void SelectedItemsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataList;
            DependencyExpressions.UpdateDependencies(control, nameof(SelectedItemsCount));
        }

        public static readonly DependencyProperty SelectedItemsCountProperty = DependencyProperty.Register(nameof(SelectedItemsCount), typeof(int), typeof(DataList), new PropertyMetadata(null, SelectedItemsCountChanged));
        #endregion


        #region ToolbarMode
        public ListToolbarMode ToolbarMode
        {
            get { return (ListToolbarMode)GetValue(ToolbarModeProperty); }
            set { SetValue(ToolbarModeProperty, value); }
        }

        public static readonly DependencyProperty ToolbarModeProperty = DependencyProperty.Register(nameof(ToolbarMode), typeof(ListToolbarMode), typeof(DataList), new PropertyMetadata(ListToolbarMode.Default));
        #endregion

        #region Query
        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register(nameof(Query), typeof(string), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region SelectionMode
        public ListViewSelectionMode SelectionMode
        {
            get { return (ListViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(ListViewSelectionMode), typeof(DataList), new PropertyMetadata(ListViewSelectionMode.Single));
        #endregion

        #region ItemsCount
        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register(nameof(ItemsCount), typeof(int), typeof(DataList), new PropertyMetadata(0));
        #endregion

        #region PageIndex
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof(PageIndex), typeof(int), typeof(DataList), new PropertyMetadata(0));
        #endregion

        #region PageSize
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(nameof(PageSize), typeof(int), typeof(DataList), new PropertyMetadata(0));
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

        public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(nameof(NewCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region RefreshCommand
        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register(nameof(RefreshCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
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

        #region SelectItemsCommand
        public ICommand SelectItemsCommand
        {
            get { return (ICommand)GetValue(SelectItemsCommandProperty); }
            set { SetValue(SelectItemsCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectItemsCommandProperty = DependencyProperty.Register(nameof(SelectItemsCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion

        #region DeselectItemsCommand
        public ICommand DeselectItemsCommand
        {
            get { return (ICommand)GetValue(DeselectItemsCommandProperty); }
            set { SetValue(DeselectItemsCommandProperty, value); }
        }

        public static readonly DependencyProperty DeselectItemsCommandProperty = DependencyProperty.Register(nameof(DeselectItemsCommand), typeof(ICommand), typeof(DataList), new PropertyMetadata(null));
        #endregion


        public bool IsSingleSelection => !IsMultipleSelection;
        static DependencyExpression IsSingleSelectionExpression = DependencyExpressions.Register(nameof(IsSingleSelection), nameof(IsMultipleSelection));

        public bool IsDataAvailable => (ItemsSource?.Cast<object>().Any() ?? false);
        static DependencyExpression IsDataAvailableExpression = DependencyExpressions.Register(nameof(IsDataAvailable), nameof(ItemsSource));

        public bool IsDataUnavailable => !IsDataAvailable;
        static DependencyExpression IsDataUnavailableExpression = DependencyExpressions.Register(nameof(IsDataUnavailable), nameof(IsDataAvailable));

        public string DataUnavailableMessage => ItemsSource == null ? "Loading..." : "No items found.";
        static DependencyExpression DataUnavailableMessageExpression = DependencyExpressions.Register(nameof(DataUnavailableMessage), nameof(ItemsSource));

        public string ItemsSelectedText => $"{SelectedItemsCount} items selected.";
        static DependencyExpression ItemsSelectedTextExpression = DependencyExpressions.Register(nameof(ItemsSelectedText), nameof(SelectedItemsCount));

        public string GetSelectionText(int count)
        {
            return $"{count} items selected.";
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsMultipleSelection)
            {
                SelectedItemsCount = listview.SelectedItems.Count;
                ToolbarMode = SelectedItemsCount > 0 ? ListToolbarMode.CancelDelete : ListToolbarMode.Cancel;
            }

            if (SelectItemsCommand?.CanExecute(e.AddedItems) ?? false)
            {
                SelectItemsCommand.Execute(e.AddedItems);
            }
            if (DeselectItemsCommand?.CanExecute(e.RemovedItems) ?? false)
            {
                DeselectItemsCommand.Execute(e.RemovedItems);
            }
        }

        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (QuerySubmittedCommand?.CanExecute(args.QueryText) ?? false)
            {
                QuerySubmittedCommand.Execute(args.QueryText);
            }
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
                    IsMultipleSelection = true;
                    ToolbarMode = ListToolbarMode.Cancel;
                    SelectionMode = ListViewSelectionMode.Multiple;
                    break;
                case ToolbarButton.Refresh:
                    if (RefreshCommand?.CanExecute(null) ?? false)
                    {
                        RefreshCommand.Execute(null);
                    }
                    break;
                case ToolbarButton.Cancel:
                    IsMultipleSelection = false;
                    ToolbarMode = ListToolbarMode.Default;
                    SelectionMode = ListViewSelectionMode.Single;
                    SelectedItem = ItemsSource?.Cast<object>().FirstOrDefault();
                    break;
            }
        }

        #region NotifyPropertyChanged
        private bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
