using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    /// MaterialControl.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialControl : UserControl
    {
        bool _isIncreasePressing = false;
        bool _isReducePressing = false;

        public MaterialControl()
        {
            InitializeComponent();
            DataContext = this;

            Increase.OnClick += Increase_OnClick;
            Increase.OnPressing += Increase_OnPressing;

            Reduce.OnClick += Reduce_OnClick;
            Reduce.OnPressing += Reduce_OnPressing;

            Maximize.OnClick += p => Value = MaxValue;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MaterialControl), new PropertyMetadata(string.Empty));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(MaterialControl), new PropertyMetadata(0));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(MaterialControl), new PropertyMetadata(0));

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(MaterialControl), new PropertyMetadata(0));

        private void Increase_OnClick(ClickEffectGrid sender)
        {
            if (Value >= MinValue && Value < MaxValue)
            {
                Value += 1;
            }
        }

        private void Reduce_OnClick(ClickEffectGrid sender)
        {
            if (Value > MinValue && Value <= MaxValue)
            {
                Value -= 1;
            }
        }

        private void Reduce_OnPressing(ClickEffectGrid sender, bool isPressing)
        {
            if (isPressing && !_isIncreasePressing && !_isReducePressing)
            {
                Task.Run(() => LoopValue(-1));
            }

            _isReducePressing = isPressing;
        }

        private void Increase_OnPressing(ClickEffectGrid sender, bool isPressing)
        {
            if (isPressing && !_isIncreasePressing && !_isReducePressing)
            {
                Task.Run(() => LoopValue(1));
            }

            _isIncreasePressing = isPressing;
        }

        private async void LoopValue(int value)
        {
            int count = 0;
            await Task.Delay(250);

            while (_isIncreasePressing || _isReducePressing)
            {
                await Task.Delay(50);

                if (count <= 20) count++;
                if (count == 10 || count == 20) value *= 10;

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (Value + value >= MinValue && Value + value <= MaxValue)
                    {
                        Value += value;
                    }
                    else if (value > 1 || value < -1)
                    {
                        value /= 10;
                    }
                }));
            } 
        }
    }
}
