using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.ViewModel;
using Ma.Terminal.SelfService.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

namespace Ma.Terminal.SelfService.View
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPageView : Page, IPageViewInterface
    {
        MainPageViewModel _viewModel = null;
        public IViewModel ViewModel => _viewModel;

        public MainPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<MainPageViewModel>();
            DataContext = _viewModel;

            InitCard.OnClick += Grid_OnClick;
            ReCard.OnClick += Grid_OnClick;

            Loaded += MainPageView_Loaded;
        }

        private void MainPageView_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Ioc.Default.GetRequiredService<Device.Light.Operator>().UnLight());

            var machine = Ioc.Default.GetRequiredService<Machine>();

            _viewModel.IsServiceAvailable = machine.Detail.Status == 1;

            Task.Run(async () =>
            {
                if (!_viewModel.IsLoading)
                {
                    _viewModel.IsLoading = true;
                    await _viewModel.Loading();
                }
            });
        }

        private void Grid_OnClick(Controls.ClickEffectGrid sender)
        {
            if (sender.Tag != null)
            {
                Ioc.Default.GetRequiredService<UserModel>().PinkupType = sender.Name;
                _viewModel.NavigationTo?.Invoke(sender.Tag as IPageViewInterface);
            }
        }

        public IPageViewInterface Init(INavigationSupport navigationParent)
        {
            _viewModel.Initialization();
            _viewModel.NavigationTo = navigationParent.NavigationTo;

            foreach (var item in navigationParent.PageList)
            {
                if (item is QueryPageView)
                {
                    InitCard.Tag = item;
                    ReCard.Tag = item;
                }
            }

            return this;
        }
    }
}
