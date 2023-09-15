using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    /// QueryPage.xaml 的交互逻辑
    /// </summary>
    public partial class QueryPageView : Page, IPageViewInterface, IBackspaceSupportView, INextPageSupportView, IErrorPageSupportView
    {
        const string CODE_PATTERN = @"^\d{6}$";
        const string PHONE_PATTERN = @"^(13[0-9]|14[5|7]|15[0|1|2|3|4|5|6|7|8|9]|17[0-9]|18[0|1|2|3|5|6|7|8|9]|19[0-9]|)\d{8}$";

        Regex _codeRegex = new Regex(CODE_PATTERN);
        Regex _phoneRegex = new Regex(PHONE_PATTERN);

        QueryPageViewModel _viewModel;
        public IViewModel ViewModel => _viewModel;

        public IPageViewInterface BackPageView { get; set; }
        public IPageViewInterface NextPageView { get; set; }
        public IPageViewInterface ErrorPageView { get; set; }

        public QueryPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<QueryPageViewModel>();
            DataContext = _viewModel;

            Title.OnBackspaceClick += () => _viewModel.NavigationTo(BackPageView);
            Keyboard.OnConfirmButtonClick += async () =>
            {
                Keyboard.IsEnabled = false;
                TextPhone.IsReadOnly = true;
                TextCode.IsReadOnly = true;

                try
                {
                    var model = await _viewModel.Query(TextPhone.Text, TextCode.Text);
                    if (model != null)
                    {
                        Ioc.Default.GetRequiredService<UserModel>().Update(model);
                        _viewModel.NavigationTo(NextPageView);
                    }
                    else
                    {
                        var errVM = ErrorPageView.ViewModel as ErrorPageViewModel;
                        errVM.ErrorType = "NoCard";
                        _viewModel.NavigationTo(ErrorPageView);
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    Keyboard.IsEnabled = true;
                    TextPhone.IsReadOnly = false;
                    TextCode.IsReadOnly = false;
                }
            };


            TextCode.GotFocus += TextBox_GotFocus;
            TextPhone.GotFocus += TextBox_GotFocus;

            TextCode.TextChanged += TextCode_TextChanged;
            TextPhone.TextChanged += TextCode_TextChanged;

            Loaded += QueryPageView_Loaded;
        }

        private void TextCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            Keyboard.SetConfirmEnable(_codeRegex.IsMatch(TextCode.Text) & _phoneRegex.IsMatch(TextPhone.Text)); 
        }

        private void QueryPageView_Loaded(object sender, RoutedEventArgs e)
        {
            TextCode.Text = string.Empty;
            TextPhone.Text = string.Empty;

            Keyboard.Focus();
            Keyboard.CurrentTextBox = TextCode;
            Keyboard.SetConfirmEnable(false);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.CurrentTextBox = sender as TextBox;
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
