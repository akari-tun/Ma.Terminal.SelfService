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
    /// InputPwdPageView.xaml 的交互逻辑
    /// </summary>
    public partial class InputPwdPageView : Page, IPageViewInterface, IBackspaceSupportView, INextPageSupportView
    {
        public enum ActionTypeEnum
        {
            EXIT,
            RESET
        }

        InputPwdPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;
        public ActionTypeEnum ActionType { get; set; }
        public IPageViewInterface BackPageView { get; set; }
        public IPageViewInterface NextPageView { get; set; }

        public InputPwdPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<InputPwdPageViewModel>();
            DataContext = _viewModel;

            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);
            Keyboard.OnConfirmButtonClick += () =>
            {
                Keyboard.IsEnabled = false;
                TextPwd.IsReadOnly = true;
                var machine = Ioc.Default.GetRequiredService<Machine>();

                try
                {
                    if (Keyboard.GetText() == machine.Password)
                    {
                        switch (ActionType)
                        {
                            case ActionTypeEnum.EXIT:
                                Application.Current.Shutdown();
                                break;
                            case ActionTypeEnum.RESET:
                                _viewModel.NavigationTo(NextPageView);
                                break;
                        }
                    }
                    else
                    {
                        ErrorMsg.Text = _viewModel.GetString("PwdIncorrectDescript");
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    Keyboard.IsEnabled = true;
                    TextPwd.IsReadOnly = false;
                }
            };

            Loaded += InputPwdPageView_Loaded;
            TextPwd.TextChanged += (s, e) =>
            {
                ErrorMsg.Text = string.Empty;
            };
        }

        private void InputPwdPageView_Loaded(object sender, RoutedEventArgs e)
        {
            TextPwd.Text = string.Empty;

            Keyboard.Focus();
            Keyboard.CurrentTextBox = TextPwd;
            Keyboard.IsPassword = true;
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
