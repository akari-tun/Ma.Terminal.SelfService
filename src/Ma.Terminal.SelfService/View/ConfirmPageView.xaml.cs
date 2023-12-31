﻿using CommunityToolkit.Mvvm.DependencyInjection;
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
    /// ConfirmPageView.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmPageView : Page, IPageViewInterface, IBackspaceSupportView, INextPageSupportView
    {
        ConfirmPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }
        public IPageViewInterface NextPageView { get; set; }

        public ConfirmPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<ConfirmPageViewModel>();
            DataContext = _viewModel;

            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);
            Confirm.OnClick += p => _viewModel.NavigationTo(NextPageView);

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
    }
}
