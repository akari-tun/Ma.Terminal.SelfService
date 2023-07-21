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
    /// TitleControl.xaml 的交互逻辑
    /// </summary>
    public partial class TitleControl : UserControl
    {
        public delegate void BackspaceClickHandler();

        public event BackspaceClickHandler OnBackspaceClick;

        public TitleControl()
        {
            InitializeComponent();

            Loaded += TitleControl_Loaded;
        }

        private void TitleControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextTitle.Text = Title;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TitleControl), new PropertyMetadata(string.Empty));

        private void ClickEffectGrid_OnClick(ClickEffectGrid sender)
        {
            OnBackspaceClick?.Invoke();
        }
    }
}
