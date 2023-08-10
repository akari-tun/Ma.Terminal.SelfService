using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Controls;
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
    /// ResetPageView.xaml 的交互逻辑
    /// </summary>
    public partial class ResetPageView : Page, IPageViewInterface, IBackspaceSupportView
    {
        Machine _machine;
        ItemsConfig _config;
        ResetPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }

        public ResetPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<ResetPageViewModel>();
            _machine = Ioc.Default.GetRequiredService<Machine>();
            _config = Ioc.Default.GetRequiredService<ItemsConfig>();

            DataContext = _viewModel;

            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);

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
            return this;
        }

        public void NavigatedTo(IModel model)
        {
        }

        private void ResetCard(ClickEffectGrid sender)
        {
            _config.Card = _machine.MaxCard;
            _machine.Detail.CardCount = _config.Card.ToString();
            _viewModel.CardSurplus = _machine.Detail.CardCount;

            _config.Save();
        }

        private void ResetInk(ClickEffectGrid sender)
        {
            _config.Ink = _machine.MaxInk;
            _machine.Detail.InkCount = _config.Ink.ToString();
            _viewModel.InkSurplus = _machine.Detail.InkCount;

            _config.Save();
        }

        private void ResetLanyard(ClickEffectGrid sender)
        {
            _config.Lanyard = _machine.MaxLanyard;
            _machine.Detail.CardRopeCover = _config.Lanyard.ToString();
            _viewModel.LanyardSurplus = _machine.Detail.CardRopeCover;

            _config.Save();
        }
    }
}
