using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.View;
using Ma.Terminal.SelfService.ViewModel;
using Ma.Terminal.SelfService.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Ma.Terminal.SelfService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isRun = true;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new MainContainerView();

#if DEBUG
            MainFrame.Height = 1080;
            MainFrame.Width = 607;
#else
            MainFrame.Height = 1920;
            MainFrame.Width = 1080;
#endif
            Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    TimeSpan ts = TimeSpan.FromTicks(DateTime.Now.Ticks);
                    Requester _api = Ioc.Default.GetRequiredService<Requester>(); ;
                    Machine _machine = Ioc.Default.GetRequiredService<Machine>();

                    do
                    {
                        TimeSpan now = TimeSpan.FromTicks(DateTime.Now.Ticks).Subtract(ts);
                        
                        if (now.TotalMinutes > 30)
                        {
                            await _api.SaveMachine(_machine.MachineNo,
                                                   _machine.Detail.CardCount,
                                                   _machine.Detail.InkCount,
                                                   _machine.Detail.CardRopeCover);

                            ts = TimeSpan.FromTicks(DateTime.Now.Ticks);
                        }

                        Thread.Sleep(100);

                    } while (isRun);
                });
            });
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
#if DEBUG
            this.Topmost = false;
#else
            this.Topmost = true;
#endif
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
        }

        protected override void OnClosed(EventArgs e)
        {
            isRun = false;
            base.OnClosed(e);
        }
    }
}
