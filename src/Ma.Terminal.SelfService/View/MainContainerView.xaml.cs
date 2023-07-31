using CommunityToolkit.Mvvm.DependencyInjection;
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
    /// MainContainerView.xaml 的交互逻辑
    /// </summary>
    public partial class MainContainerView : Page
    {
        MainContainerViewModel _viewModel = null;
        public IViewModel ViewModel => _viewModel;

        private MainPageView _mainPage = new MainPageView();
        private QueryPageView _queryPage = new QueryPageView();
        private ErrorPageView _errorPage = new ErrorPageView();
        private ConfirmPageView _confirmPage = new ConfirmPageView();
        private WaitPageView _waitPage = new WaitPageView();
        private TakePageView _takePage = new TakePageView();

        public MainContainerView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<MainContainerViewModel>();
            DataContext = _viewModel;

            _viewModel.PageContainer = PageFrame;
            _viewModel.PageList = new List<IPageViewInterface>
            {
                _mainPage,
                _queryPage,
                _errorPage,
                _confirmPage,
                _waitPage,
                _takePage
            };

            foreach (var item in _viewModel.PageList)
            {
                item.Init(_viewModel);
            }

            Loaded += MainContainerView_Loaded;

            _queryPage.BackPageView = _mainPage;
            _queryPage.NextPageView = _confirmPage;
            _queryPage.ErrorPageView = _errorPage;
            _errorPage.BackPageView = _queryPage;
            _confirmPage.BackPageView = _queryPage;
            _confirmPage.NextPageView = _waitPage;
            _waitPage.BackPageView = _confirmPage;
            _waitPage.NextPageView = _takePage;
            _takePage.BackPageView = _mainPage;
            _takePage.NextPageView = _mainPage;
        }

        private void MainContainerView_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.NavigationTo(_viewModel.PageList[0]);
        }
    }
}
