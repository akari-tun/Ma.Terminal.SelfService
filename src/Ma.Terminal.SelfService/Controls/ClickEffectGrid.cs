using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ma.Terminal.SelfService.Controls
{
    public class ClickEffectGrid : Grid
    {
        public delegate void ClickEffectGridOnClickHandler(ClickEffectGrid sender);

        static Thickness DEFAULT_MARGIN_THICKNESS = new Thickness(0);
        static Thickness HOVER_MARGIN_THICKNESS = new Thickness(2);

        public event ClickEffectGridOnClickHandler OnClick;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.Margin = HOVER_MARGIN_THICKNESS;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.Margin = DEFAULT_MARGIN_THICKNESS;
            base.OnMouseLeftButtonUp(e);

            OnClick?.Invoke(this);
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            this.Margin = HOVER_MARGIN_THICKNESS;
            base.OnTouchDown(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            this.Margin = DEFAULT_MARGIN_THICKNESS;
            base.OnTouchUp(e);

            OnClick?.Invoke(this);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Margin = DEFAULT_MARGIN_THICKNESS;
            base.OnMouseLeave(e);
        }

        protected override void OnTouchLeave(TouchEventArgs e)
        {
            this.Margin = DEFAULT_MARGIN_THICKNESS;
            base.OnTouchLeave(e);
        }
    }
}
