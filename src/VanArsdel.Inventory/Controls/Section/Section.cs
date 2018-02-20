using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory.Controls
{
    public sealed class Section : ContentControl
    {
        public event RoutedEventHandler HeaderButtonClick;

        private Grid _grid = null;
        private IconLabelButton _button = null;

        public Section()
        {
            DefaultStyleKey = typeof(Section);
        }

        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private static void HeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Section;
            control.UpdateControl();
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(Section), new PropertyMetadata(null, HeaderChanged));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Section), new PropertyMetadata(null));
        #endregion

        #region HeaderButtonGlyph
        public string HeaderButtonGlyph
        {
            get { return (string)GetValue(HeaderButtonGlyphProperty); }
            set { SetValue(HeaderButtonGlyphProperty, value); }
        }

        private static void HeaderButtonGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Section;
            control.UpdateControl();
        }

        public static readonly DependencyProperty HeaderButtonGlyphProperty = DependencyProperty.Register("HeaderButtonGlyph", typeof(string), typeof(Section), new PropertyMetadata(null, HeaderButtonGlyphChanged));
        #endregion

        #region HeaderButtonLabel
        public string HeaderButtonLabel
        {
            get { return (string)GetValue(HeaderButtonLabelProperty); }
            set { SetValue(HeaderButtonLabelProperty, value); }
        }

        private static void HeaderButtonLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Section;
            control.UpdateControl();
        }

        public static readonly DependencyProperty HeaderButtonLabelProperty = DependencyProperty.Register("HeaderButtonLabel", typeof(string), typeof(Section), new PropertyMetadata(null, HeaderButtonLabelChanged));
        #endregion

        #region Footer
        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(object), typeof(Section), new PropertyMetadata(null));
        #endregion

        #region FooterTemplate
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(Section), new PropertyMetadata(null));
        #endregion

        private void UpdateControl()
        {
            if (_grid != null)
            {
                _grid.RowDefinitions[0].Height = Header == null ? GridLengths.Zero : GridLengths.Auto;
                _grid.RowDefinitions[2].Height = Footer == null ? GridLengths.Zero : GridLengths.Auto;
                _button.Visibility = String.IsNullOrEmpty($"{HeaderButtonGlyph}{HeaderButtonLabel}") ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _grid = base.GetTemplateChild("grid") as Grid;

            _button = base.GetTemplateChild("button") as IconLabelButton;
            _button.Click += OnClick;

            UpdateControl();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            HeaderButtonClick?.Invoke(this, e);
        }
    }
}
