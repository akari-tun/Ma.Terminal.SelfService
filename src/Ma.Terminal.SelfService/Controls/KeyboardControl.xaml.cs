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

        public FunctionButtonClickHandler OnConfirmButtonClick;

        static SolidColorBrush ENABLE_BACKGROUND_BRUSH = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC9, 0x41));
        static SolidColorBrush DISABLE_BACKGROUND_BRUSH = new SolidColorBrush(Color.FromArgb(0xFF, 0xEB, 0xEB, 0xEB));
        static SolidColorBrush ENABLE_TEXT_BRUSH = new SolidColorBrush(Color.FromArgb(0xFF, 0x0F, 0x18, 0x26));
        static SolidColorBrush DISABLE_TEXT_BRUSH = new SolidColorBrush(Color.FromArgb(0xFF, 0x6E, 0x75, 0x80));

        string _inputed = string.Empty;

        TextBox _currentTextBox;

        public TextBox CurrentTextBox
        {
            get => _currentTextBox;
            set
            {
                _currentTextBox = value;
                _inputed = _currentTextBox.Text;
            }
        }

        public string GetText()
        {
            return IsPassword ? _inputed : _currentTextBox.Text;
        }

        public bool IsPassword { get; set; } = false;

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

            Clear.OnClick += p =>
            {
                if (_currentTextBox != null)
                {
                    _currentTextBox.Text = string.Empty;
                    _inputed = string.Empty;
                }
            };
            Backspace.OnClick += p =>
            {
                if (_currentTextBox != null && _currentTextBox.Text.Length > 0)
                {
                    var index = 0;

                    if (IsPassword)
                    {
                        index = _currentTextBox.SelectionStart > _inputed.Length ? _inputed.Length : _currentTextBox.SelectionStart;
                        var l_tmp = _inputed.Substring(0, index - 1);
                        var r_tmp = _inputed.Substring(index, _inputed.Length - index);
                        _inputed = l_tmp + r_tmp;

                        StringBuilder sb = new StringBuilder(6);
                        for (int i = 0; i < _inputed.Length; i++)
                        {
                            sb.Append("*");
                        }
                        _currentTextBox.Text = sb.ToString();
                    }
                    else
                    {
                        index = _currentTextBox.SelectionStart;
                        var l_tmp = _currentTextBox.Text.Substring(0, index - 1);
                        var r_tmp = _currentTextBox.Text.Substring(index, _currentTextBox.Text.Length - index);
                        _currentTextBox.Text = l_tmp + r_tmp;
                    }

                    _currentTextBox.SelectionStart = --index > 0 ? index : 0;
                }
            };
        }

        public void SetConfirmEnable(bool enable)
        {
            Confirm.IsEnabled = enable;
            Confirm.Background = enable ? ENABLE_BACKGROUND_BRUSH : DISABLE_BACKGROUND_BRUSH;
            ConfirmText.Foreground = enable ? ENABLE_TEXT_BRUSH : DISABLE_TEXT_BRUSH;
        }

        private void DigitClick(ClickEffectGrid sender)
        {
            if (_currentTextBox != null && _currentTextBox.Text.Length < _currentTextBox.MaxLength)
            {
                if (IsPassword)
                {
                    var index = _currentTextBox.SelectionStart > _inputed.Length ? _inputed.Length : _currentTextBox.SelectionStart;
                    var l_tmp = _inputed.Substring(0, index);
                    var r_tmp = _inputed.Substring(index, _inputed.Length - index);
                    _inputed = l_tmp + sender.Tag.ToString() + r_tmp;

                    StringBuilder sb = new StringBuilder(6);
                    for (int i = 0; i < _inputed.Length; i++)
                    {
                        sb.Append("*");
                    }
                    _currentTextBox.Text = sb.ToString();

                    _currentTextBox.SelectionStart = ++index;
                }
                else
                {
                    var index = _currentTextBox.SelectionStart;
                    var l_tmp = _currentTextBox.Text.Substring(0, index);
                    var r_tmp = _currentTextBox.Text.Substring(index, _currentTextBox.Text.Length - index);
                    _currentTextBox.Text = l_tmp + sender.Tag.ToString() + r_tmp;
                    _currentTextBox.SelectionStart = ++index;
                }
            }
        }
    }
}
