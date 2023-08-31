using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private ItemsConfig _config;
        private Requester _api;
        private Machine _machine;
        private Device.Printer.Operator _printer;
        Logger _logger = LogManager.GetCurrentClassLogger();

        public Action<IPageViewInterface> NavigationTo;

        bool _isServiceAvailable;
        public bool IsServiceAvailable
        {
            get => _isServiceAvailable;
            set
            {
                SetProperty(ref _isServiceAvailable, value);
            }
        }

        string _error;
        public string Error
        {
            get => _error;
            set
            {
                SetProperty(ref _error, value);
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        public MainPageViewModel(Machine machine, Requester api, ItemsConfig config, Device.Printer.Operator printer) : base()
        {
            _machine = machine;
            _api = api;
            _config = config;
            _printer = printer;
        }

        public override void Initialization()
        {
        }

        public async Task Loading()
        {
            do
            {
                try
                {

                    if (_config.Card == 0 || _config.Ink == 0 || _config.Lanyard == 0)
                    {
                        _logger.Info($"Meterial low {_config}");
                        _machine.Detail.Status = 0;
                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            IsServiceAvailable = _machine.Detail.Status == 1;
                            Error = IsServiceAvailable ? string.Empty : GetString("NoMaterial");
                        }));
                    }
#if DEBUG
#else
                    else if (!_printer.IsReady())
                    {
                        _machine.Detail.Status = 0;
                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            IsServiceAvailable = _machine.Detail.Status == 1;
                            Error = IsServiceAvailable ? string.Empty : _printer.LastError;
                        }));
                    }
#endif
                    else
                    {
                        var detail = await _api.GetMachineDetail();

                        if (detail != null)
                        {
                            _machine.Detail.ProjectId = detail.ProjectId;
                            _machine.Detail.Address = detail.Address;
                            _machine.Detail.Status = detail.Status;

                            IsLoading = _machine.Detail.Status != 1;
                        }
                        else
                        {
                            _machine.Detail.Status = 0;
                            _logger.Info($"GetMachineDetail => {_api.LastMessage}");
                        }

                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            IsServiceAvailable = _machine.Detail.Status == 1;
                            Error = IsServiceAvailable ? string.Empty : _api.LastMessage;
                        }));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);
                }

                for (int i = 0; i < 1000; i++)
                {
                    if (!IsLoading) break;
                    await Task.Delay(10);
                }
            } while (!IsServiceAvailable || IsLoading);

            IsLoading = false;
        }
    }
}
