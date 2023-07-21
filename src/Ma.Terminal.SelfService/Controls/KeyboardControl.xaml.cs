using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ma.Terminal.SelfService.Controls
{
    /// <summary>
    /// KeyboardControl.xaml 的交互逻辑
    /// </summary>
    public partial class KeyboardControl : UserControl
    {
        public delegate void DigitButtonClickHandler(string value);
        public delegate void FunctionButtonClickHandler();

        public DigitButtonClickHandler OnDigitButtonClick;
        public FunctionButtonClickHandler OnConfirmButtonClick;
        public FunctionButtonClickHandler OnClearButtonClick;
        public FunctionButtonClickHandler OnBackspaceButtonClick;

        public KeyboardControl()
        {
            InitializeComponent();

            One.OnClick += DigitClick;
            Two.OnClick += DigitClick;
            Three.OnClick += DigitClick;
            Four.OnClick += DigitClick;
            Five.OnClick += DigitClick;
            Six.OnClick += DigitClick;
            Seven.OnClick += DigitClick;
            Eight.OnClick += DigitClick;
            Nine.OnClick += DigitClick;
            Zero.OnClick += DigitClick;
            //Dot.OnClick += DigitClick;

            Confirm.OnClick += p => OnConfirmButtonClick?.Invoke();
            Clear.OnClick += p => OnClearButtonClick?.Invoke();
            Backspace.OnClick += p => OnBackspaceButtonClick?.Invoke();
        }

        private void DigitClick(ClickEffectGrid sender)
        {
            OnDigitButtonClick?.Invoke(sender.Tag.ToString());
        }
    }
}
