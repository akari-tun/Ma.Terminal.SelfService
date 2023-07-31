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
    /// ErrorPageView.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorPageView : Page, IPageViewInterface, IBackspaceSupportView
    {
        ErrorPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }

        public ErrorPageView()
        {
            InitializeComponent();


            _viewModel = Ioc.Default.GetRequiredService<ErrorPageViewModel>();
            DataContext = _viewModel;

            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);

            NoCard.Visibility = Visibility.Visible;
            HaveCard.Visibility = Visibility.Collapsed;
        }

        public IPageViewInterface Init(INavigationSupport navigationParent)
        {
            _viewModel.Initialization();
            _viewModel.NavigationTo = navigationParent.NavigationTo;
            return this;
        }

        public void NavigatedTo(IModel model)
        {
            Ioc.Default.GetRequiredService<ErrorPageViewModel>();
            NoCard.Visibility = Visibility.Collapsed;
            HaveCard.Visibility = Visibility.Collapsed;

            if (NoCard.Name == _viewModel.ErrorType) NoCard.Visibility = Visibility.Visible;
            if (HaveCard.Name == _viewModel.ErrorType) HaveCard.Visibility = Visibility.Visible;
        }
    }
}
