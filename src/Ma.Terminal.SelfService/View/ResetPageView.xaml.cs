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
        ResetPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }

        public ResetPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<ResetPageViewModel>();
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
            var machine = Ioc.Default.GetRequiredService<Machine>();
            machine.Detail.CardCount = machine.MaxCard.ToString();

            _viewModel.CardSurplus = machine.Detail.CardCount;
        }

        private void ResetInk(ClickEffectGrid sender)
        {
            var machine = Ioc.Default.GetRequiredService<Machine>();
            machine.Detail.InkCount = machine.MaxInk.ToString();

            _viewModel.InkSurplus = machine.Detail.InkCount;
        }

        private void ResetLanyard(ClickEffectGrid sender)
        {
            var machine = Ioc.Default.GetRequiredService<Machine>();
            machine.Detail.CardRopeCover = machine.MaxLanyard.ToString();

            _viewModel.LanyardSurplus = machine.Detail.CardRopeCover;
        }
    }
}
