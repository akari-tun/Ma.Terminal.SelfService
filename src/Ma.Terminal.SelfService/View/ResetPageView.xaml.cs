using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Controls;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.ViewModel;
using Ma.Terminal.SelfService.WebApi;
using NLog;
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
    /// ResetPageView.xaml 的交互逻辑
    /// </summary>
    public partial class ResetPageView : Page, IPageViewInterface, IBackspaceSupportView
    {
        Machine _machine;
        ItemsConfig _config;
        ResetPageViewModel _viewModel;
        private Requester _api;
        Logger _logger = LogManager.GetCurrentClassLogger();
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }

        public ResetPageView()
        {
            InitializeComponent();

            _api = Ioc.Default.GetRequiredService<Requester>(); ;
            _viewModel = Ioc.Default.GetRequiredService<ResetPageViewModel>();
            _machine = Ioc.Default.GetRequiredService<Machine>();
            _config = Ioc.Default.GetRequiredService<ItemsConfig>();

            DataContext = _viewModel;

            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);
            Cancel.OnClick += p => _viewModel.NavigationTo(BackPageView);
            Enter.OnClick += p =>
            {
                _config.Card = Card.Value;
                _machine.Detail.CardCount = _config.Card.ToString();
                _viewModel.CardSurplus = int.Parse(_machine.Detail.CardCount);

                _config.Ink = Ink.Value;
                _machine.Detail.InkCount = _config.Ink.ToString();
                _viewModel.InkSurplus = int.Parse(_machine.Detail.InkCount);

                _config.Lanyard = Lanyard.Value;
                _machine.Detail.CardRopeCover = _config.Lanyard.ToString();
                _viewModel.LanyardSurplus = int.Parse(_machine.Detail.CardRopeCover);

                Task.Run(async () =>
                {
                    await _api.SaveMachine(_machine.MachineNo,
                                           _machine.Detail.CardCount,
                                           _machine.Detail.InkCount,
                                           _machine.Detail.CardRopeCover);
                });

                _config.Save();
                _logger.Info($"Material resett {_config}");
                _viewModel.NavigationTo(BackPageView);
            };

            Loaded += ConfirmPageView_Loaded;
        }
        private void ConfirmPageView_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialization();
        }

        public IPageViewInterface Init(INavigationSupport navigationParent)
        {
            _viewModel.Initialization();
            _viewModel.NavigationTo = navigationParent.NavigationTo;

            Card.MaxValue = _viewModel.MaxCardValue;
            Ink.MaxValue = _viewModel.MaxInkValue;
            Lanyard.MaxValue = _viewModel.MaxLanyardValue;

            Card.Value = _viewModel.CardSurplus;
            Ink.Value = _viewModel.InkSurplus;
            Lanyard.Value = _viewModel.LanyardSurplus;

            return this;
        }

        public void NavigatedTo(IModel model)
        {
        }
    }
}
