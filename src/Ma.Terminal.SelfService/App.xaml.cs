using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Device.Reader;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.Resource;
using Ma.Terminal.SelfService.ViewModel;
using Ma.Terminal.SelfService.WebApi;
using Microsoft.Extensions.Configuration;
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

            ConfigurationBuilder cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddJsonFile("machine.json", optional: false, reloadOnChange:false);
            IConfigurationRoot cfgRoot = cfgBuilder.Build();

            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton(typeof(MainContainerViewModel))
                .AddSingleton(typeof(MainPageViewModel))
                .AddSingleton(typeof(QueryPageViewModel))
                .AddSingleton(typeof(ErrorPageViewModel))
                .AddSingleton(typeof(ConfirmPageViewModel))
                .AddSingleton(typeof(WaitPageViewModel))
                .AddSingleton(typeof(TakePageViewModel))
                .AddSingleton(typeof(Operator))
                .AddSingleton(new UserModel())
                .AddSingleton(new Machine()
                {
                    MachineNo = cfgRoot.GetSection("MachineNo").Value,
                    ApiUrl = cfgRoot.GetSection("ApiUrl").Value
                })
                .AddSingleton(typeof(Requester))
                .BuildServiceProvider());

            var path = $"pack://application:,,,/Resource/String.xaml";
            Resources.MergedDictionaries[0].Source = new Uri(path);
            ResourceManager.Instance.StringResourceDictionary = Resources.MergedDictionaries[0];

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
            });
        }
    }
}
