using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.ViewModel;
using Ma.Terminal.SelfService.WebApi;
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
            ErrorMsg.Text = string.Empty;
            InitCard.IsEnabled = false;
            ReCard.IsEnabled = false;

            Task.Run(async () =>
            {
                var requester = Ioc.Default.GetRequiredService<Requester>();
                var machine = Ioc.Default.GetRequiredService<Machine>();
                var detail = await requester.GetMachineDetail();

                if (detail != null)
                {
                    machine.Detail = new Detail()
                    {
                        ProjectId = detail.ProjectId,
                        Address = detail.Address,
                        CardCount = detail.CardCount,
                        InkCount = detail.InkCount,
                        CardRopeCover = detail.CardRopeCover,
                        Status = detail.Status
                    };
                }

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ErrorMsg.Text = machine.Detail.Status == 1 ? string.Empty : _viewModel.GetString("NoServiceDescript");
                    InitCard.IsEnabled = machine.Detail.Status == 1;
                    ReCard.IsEnabled = machine.Detail.Status == 1;
                }));
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
