using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Resource;
using Ma.Terminal.SelfService.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Ma.Terminal.SelfService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "OnlyRun");
            if (mutex.WaitOne(0, false))
            {
                base.OnStartup(e);
            }
            else
            {
                this.Shutdown();
            }

            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton(typeof(MainContainerViewModel))
                .AddSingleton(typeof(MainPageViewModel))
                .AddSingleton(typeof(QueryPageViewModel))
                .AddSingleton(typeof(ErrorPageViewModel))
                .AddSingleton(typeof(ConfirmPageViewModel))
                .AddSingleton(typeof(WaitPageViewModel))
                .AddSingleton(typeof(TakePageViewModel))
                .BuildServiceProvider());

            var path = $"pack://application:,,,/Resource/String.xaml";
            Resources.MergedDictionaries[0].Source = new Uri(path);
            ResourceManager.Instance.StringResourceDictionary = Resources.MergedDictionaries[0];
        }
    }
}
