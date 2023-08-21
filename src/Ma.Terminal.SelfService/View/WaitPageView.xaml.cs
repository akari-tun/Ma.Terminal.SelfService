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
    /// WaitPageView.xaml 的交互逻辑
    /// </summary>
    public partial class WaitPageView : Page, IPageViewInterface, IBackspaceSupportView, INextPageSupportView, IErrorPageSupportView
    {
        WaitPageViewModel _viewModel;
        ErrorPageViewModel _errViewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }
        public IPageViewInterface NextPageView { get; set; }
        public IPageViewInterface ErrorPageView { get; set; }

        public WaitPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<WaitPageViewModel>();
            _errViewModel = Ioc.Default.GetRequiredService<ErrorPageViewModel>();
            DataContext = _viewModel;

            _viewModel.OnCardPrinted += (r, m) =>
            {
                if (r)
                {
                    Application.Current.Dispatcher.BeginInvoke(_viewModel.NavigationTo, NextPageView);
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _errViewModel.ErrMsg = $"制卡失败，{m}";
                        _errViewModel.ErrorType = "ErrorMessage";
                        _viewModel.NavigationTo(ErrorPageView);
                    }));
                }
            };
            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);
        }

        public IPageViewInterface Init(INavigationSupport navigationParent)
        {
            _viewModel.Initialization();
            _viewModel.NavigationTo = navigationParent.NavigationTo;
            WaitImage.Visibility = Visibility.Visible;
            return this;
        }

        public void NavigatedTo(IModel model)
        {
            _viewModel.PrintCard();
        }
    }
}
