using System;
using System.ComponentModel;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory.Controls
{
    public sealed partial class Details : UserControl, INotifyExpressionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Details()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            DependencyExpressions.Initialize(this);
        }

        static private readonly DependencyExpressions DependencyExpressions = new DependencyExpressions();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ElementSet.Children<LabelTextBox>(container).GotFocus += OnInputGotFocus;
            ElementSet.Children<LabelComboBox>(container).GotFocus += OnInputGotFocus;
        }

        #region IsEditMode*
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        private static void IsEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Details;
            DependencyExpressions.UpdateDependencies(control, nameof(IsEditMode));
            control.UpdateEditMode();
        }

        public static readonly DependencyProperty IsEditModeProperty = DependencyProperty.Register(nameof(IsEditMode), typeof(bool), typeof(Details), new PropertyMetadata(null, IsEditModeChanged));
        #endregion

        #region DetailsContent
        public object DetailsContent
        {
            get { return (object)GetValue(DetailsContentProperty); }
            set { SetValue(DetailsContentProperty, value); }
        }

        public static readonly DependencyProperty DetailsContentProperty = DependencyProperty.Register(nameof(DetailsContent), typeof(object), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region DetailsTemplate
        public DataTemplate DetailsTemplate
        {
            get { return (DataTemplate)GetValue(DetailsTemplateProperty); }
            set { SetValue(DetailsTemplateProperty, value); }
        }

        public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register(nameof(DetailsTemplate), typeof(DataTemplate), typeof(Details), new PropertyMetadata(null));
        #endregion


        #region EditCommand
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(nameof(EditCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region SaveCommand
        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(nameof(SaveCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        #region CancelCommand
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(Details), new PropertyMetadata(null));
        #endregion

        public DetailToolbarMode ToolbarMode => IsEditMode ? DetailToolbarMode.CancelSave : DetailToolbarMode.Default;
        static DependencyExpression ToolbarModeExpression = DependencyExpressions.Register(nameof(ToolbarMode), nameof(IsEditMode));

        private void OnToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.Edit:
                    EditCommand?.TryExecute();
                    break;
                case ToolbarButton.Delete:
                    DeleteCommand?.TryExecute();
                    break;
                case ToolbarButton.Save:
                    SaveCommand?.TryExecute();
                    break;
                case ToolbarButton.Cancel:
                    CancelCommand?.TryExecute();
                    break;
            }
        }

        private void OnInputGotFocus(object sender, RoutedEventArgs e)
        {
            if (!IsEditMode)
            {
                EditCommand?.TryExecute();
            }
        }

        private void UpdateEditMode()
        {
            ElementSet.Children<LabelTextBox>(container).ForEach(c => c.Mode = IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
            ElementSet.Children<LabelComboBox>(container).ForEach(c => c.Mode = IsEditMode ? TextEditMode.ReadWrite : TextEditMode.Auto);
            if (!IsEditMode)
            {
                Focus(FocusState.Programmatic);
            }
        }

        #region NotifyPropertyChanged
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
