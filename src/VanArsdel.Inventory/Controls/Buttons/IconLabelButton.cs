﻿using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VanArsdel.Inventory.Controls
{
    public sealed class IconLabelButton : Control
    {
        public event RoutedEventHandler Click;

        private Button _button = null;

        public IconLabelButton()
        {
            DefaultStyleKey = typeof(IconLabelButton);
        }

        #region Orientation
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(IconLabelButton), new PropertyMetadata(Orientation.Horizontal));
        #endregion

        #region Glyph
        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(IconLabelButton), new PropertyMetadata(null));
        #endregion

        #region GlyphSize
        public double GlyphSize
        {
            get { return (double)GetValue(GlyphSizeProperty); }
            set { SetValue(GlyphSizeProperty, value); }
        }

        public static readonly DependencyProperty GlyphSizeProperty = DependencyProperty.Register("GlyphSize", typeof(double), typeof(IconLabelButton), new PropertyMetadata(0.0));
        #endregion

        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(IconLabelButton), new PropertyMetadata(null));
        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _button = base.GetTemplateChild("button") as Button;
            _button.Click += OnClick;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
