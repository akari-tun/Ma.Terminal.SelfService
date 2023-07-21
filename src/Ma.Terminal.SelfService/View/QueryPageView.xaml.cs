using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.ViewModel;
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

namespace Ma.Terminal.SelfService.View
{
    /// <summary>
    /// QueryPage.xaml 的交互逻辑
    /// </summary>
    public partial class QueryPageView : Page, IPageViewInterface, IBackspaceSupportView, INextPageSupportView
    {
        TextBox _currentTextBox = null;

        QueryPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }
        public IPageViewInterface NextPageView { get; set; }

        public QueryPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<QueryPageViewModel>();
            DataContext = _viewModel;

            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);
            Keyboard.OnConfirmButtonClick += () => _viewModel.NavigationTo(NextPageView);
            Keyboard.OnClearButtonClick += () =>
            {
                if (_currentTextBox != null)
                {
                    _currentTextBox.Text = string.Empty;
                }
            };
            Keyboard.OnBackspaceButtonClick += () =>
            {
                if (_currentTextBox != null && _currentTextBox.Text.Length > 0)
                {
                    _currentTextBox.Text = _currentTextBox.Text.Substring(0, _currentTextBox.Text.Length - 1);
                }
            };
            Keyboard.OnDigitButtonClick += value =>
            {
                if (_currentTextBox != null && _currentTextBox.Text.Length < _currentTextBox.MaxLength)
                {
                    var index = _currentTextBox.SelectionStart;
                    var l_tmp = _currentTextBox.Text.Substring(0, index);
                    var r_tmp = _currentTextBox.Text.Substring(index, _currentTextBox.Text.Length - index);
                    _currentTextBox.Text = l_tmp + value + r_tmp;
                    _currentTextBox.SelectionStart = ++index;
                }
            };

            TextCode.GotFocus += TextBox_GotFocus;
            TextPhone.GotFocus += TextBox_GotFocus;

            Loaded += QueryPageView_Loaded;
        }

        private void QueryPageView_Loaded(object sender, RoutedEventArgs e)
        {
            TextCode.Focus();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentTextBox = sender as TextBox;
        }

        public IPageViewInterface Init(INavigationSupport navigationParent)
        {
            _viewModel.Initialization();
            _viewModel.NavigationTo = navigationParent.NavigationTo;
            return this;
        }

        public void NavigatedTo(IModel model)
        {
        }
    }
}
