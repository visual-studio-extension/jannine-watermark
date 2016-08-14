﻿using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jannine.Watermark.Core
{
    class LogoAdornment
    {
        private IAdornmentLayer _adornmentLayer;
        private Image _adornment;
        private readonly double _initOpacity;
        private double _currentOpacity;

        public LogoAdornment(IWpfTextView view, string imageName, bool isVisible, double initOpacity)
        {
            _adornmentLayer = view.GetAdornmentLayer(LogoLayer.LayerName);
            _currentOpacity = isVisible ? _initOpacity : 0;
            _initOpacity = initOpacity;

            CreateImage(imageName);

            view.ViewportHeightChanged += SetAdornmentLocation;
            view.ViewportWidthChanged += SetAdornmentLocation;
            VisibilityChanged += ToggleVisibility;

            if (_adornmentLayer.IsEmpty)
                _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _adornment, null);
        }

        private void ToggleVisibility(object sender, bool isVisible)
        {
            _adornment.Opacity = isVisible ? _initOpacity : 0;
            _currentOpacity = _adornment.Opacity;
        }

        private void CreateImage(string imageName)
        {
            _adornment = new Image();
            //_adornment.Source = BitmapFrame.Create(new Uri("pack://application:,,,/Jannine.Watermark;component/_Shared/Resources/Watermarks/" + imageName, UriKind.RelativeOrAbsolute));
            _adornment.Source = BitmapFrame.Create(new Uri("http://www.jwofficial.com/wp-content/uploads/2013/12/IMG_20823-280x280.jpg"));
            _adornment.ToolTip = Resource.ClickToToggleVisibility;
            _adornment.Opacity = _currentOpacity;
            _adornment.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);

            _adornment.MouseEnter += (s, e) => { _adornment.Opacity = 1D; };
            _adornment.MouseLeave += (s, e) => { _adornment.Opacity = _currentOpacity; };
            _adornment.MouseLeftButtonUp += (s, e) => { OnVisibilityChanged(_currentOpacity == 0); };
        }

        private void SetAdornmentLocation(object sender, EventArgs e)
        {
            IWpfTextView view = (IWpfTextView)sender;
            Canvas.SetLeft(_adornment, view.ViewportRight - _adornment.Source.Width - 20);
            Canvas.SetTop(_adornment, view.ViewportBottom - _adornment.Source.Height - 20);
        }

        public static event EventHandler<bool> VisibilityChanged;

        private static void OnVisibilityChanged(bool isVisible)
        {
            if (VisibilityChanged != null)
                VisibilityChanged(null, isVisible);
        }
    }
}
