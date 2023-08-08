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
    public partial class WaitPageView : Page, IPageViewInterface, IBackspaceSupportView, INextPageSupportView
    {
        WaitPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }
        public IPageViewInterface NextPageView { get; set; }

        public WaitPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<WaitPageViewModel>();
            DataContext = _viewModel;

            _viewModel.OnCardPrinted += (r, m) =>
            {
                if (r)
                {
                    Application.Current.Dispatcher.BeginInvoke(_viewModel.NavigationTo, NextPageView);
                }
                else
                {
                    _viewModel.ErrMsg = "制卡失败，请联系管理员！";
                    WaitImage.Visibility = Visibility.Collapsed;
                    ErrorMessage.Visibility = Visibility.Visible;
                }

            };
            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);
        }

        public IPageViewInterface Init(INavigationSupport navigationParent)
        {
            _viewModel.Initialization();
            _viewModel.NavigationTo = navigationParent.NavigationTo;
            WaitImage.Visibility = Visibility.Visible;
            ErrorMessage.Visibility = Visibility.Collapsed;
            return this;
        }

        public void NavigatedTo(IModel model)
        {
            _viewModel.PrintCard();
        }
    }
}
