using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.Resource;
using Ma.Terminal.SelfService.ViewModel;
using Ma.Terminal.SelfService.WebApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ma.Terminal.SelfService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;
        Logger _logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "OnlyRun");
            if (mutex.WaitOne(0, false))
            {
                _logger.Info($"App is OnStartup...");
                base.OnStartup(e);
            }
            else
            {
                _logger.Info($"App is running in anther thread!");
                this.Shutdown();
            }

            ConfigurationBuilder cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddJsonFile("machine.json", optional: false, reloadOnChange:false);
            IConfigurationRoot cfgRoot = cfgBuilder.Build();

            var config = ItemsConfig.Read();
            _logger.Info($"Material config loaded {config}");

            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton(typeof(MainContainerViewModel))
                .AddSingleton(typeof(MainPageViewModel))
                .AddSingleton(typeof(QueryPageViewModel))
                .AddSingleton(typeof(ErrorPageViewModel))
                .AddSingleton(typeof(ConfirmPageViewModel))
                .AddSingleton(typeof(WaitPageViewModel))
                .AddSingleton(typeof(TakePageViewModel))
                .AddSingleton(typeof(ResetPageViewModel))
                .AddSingleton(typeof(InputPwdPageViewModel))
                .AddSingleton(typeof(Device.Printer.Operator))
                .AddSingleton(typeof(Device.Reader.Operator))
                .AddSingleton(new UserModel())
                .AddSingleton(new Machine()
                {
                    MachineNo = cfgRoot.GetSection("MachineNo").Value,
                    ApiUrl = cfgRoot.GetSection("ApiUrl").Value,
                    PrinterName = cfgRoot.GetSection("PrinterName").Value,
                    LanyardPort = int.Parse(cfgRoot.GetSection("LanyardPort").Value),
                    LanyardBaudrate = int.Parse(cfgRoot.GetSection("LanyardBaudrate").Value),
                    LightPort = int.Parse(cfgRoot.GetSection("LightPort").Value),
                    LightBaudrate = int.Parse(cfgRoot.GetSection("LightBaudrate").Value),
                    MaxCard = int.Parse(cfgRoot.GetSection("MaxCard").Value),
                    MaxInk = int.Parse(cfgRoot.GetSection("MaxInk").Value),
                    MaxLanyard = int.Parse(cfgRoot.GetSection("MaxLanyard").Value),
                    Password = cfgRoot.GetSection("Password").Value,
                    Detail = new Detail()
                    {
                        ProjectId = string.Empty,
                        Address = string.Empty,
                        CardCount = config.Card.ToString(),
                        InkCount = config.Ink.ToString(),
                        CardRopeCover = config.Lanyard.ToString(),
                        Status = 0
                    }
                })
                .AddSingleton(typeof(Requester))
                .AddSingleton(new Device.Lanyard.Operator()
                { 
                    Port = int.Parse(cfgRoot.GetSection("LanyardPort").Value),
                    Baudrate = int.Parse(cfgRoot.GetSection("LanyardBaudrate").Value)
                })
                .AddSingleton(new Device.Light.Operator()
                {
                    Port = int.Parse(cfgRoot.GetSection("LightPort").Value),
                    Baudrate = int.Parse(cfgRoot.GetSection("LightBaudrate").Value)
                })
                .AddSingleton(config)
                .BuildServiceProvider());

            var path = $"pack://application:,,,/Resource/String.xaml";
            Resources.MergedDictionaries[0].Source = new Uri(path);
            ResourceManager.Instance.StringResourceDictionary = Resources.MergedDictionaries[0];

            Ioc.Default.GetRequiredService<Device.Lanyard.Operator>().Init();
            Ioc.Default.GetRequiredService<Device.Light.Operator>().Init();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var model = Ioc.Default.GetRequiredService<MainContainerViewModel>();
            model.IsCheckRunning = false;

            base.OnExit(e);
        }

        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                _logger.Error(e.Exception.Message);
                _logger.Error(e.Exception.StackTrace);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    _logger.Error(exception.Message);
                    _logger.Error(exception.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
        }
    }
}
