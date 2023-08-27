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

        public MainPageViewModel(Machine machine, Requester api, ItemsConfig config) : base()
        {
            _machine = machine;
            _api = api;
            _config = config;
        }

        public override void Initialization()
        {
        }

        public async Task Loading()
        {
            try
            {
                do
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
                    else
                    {
                        var detail = await _api.GetMachineDetail();

                        if (detail != null)
                        {
                            _machine.Detail = new Detail()
                            {
                                ProjectId = detail.ProjectId,
                                Address = detail.Address,
                                Status = detail.Status
                            };

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


                    for (int i = 0; i < 1000; i++)
                    {
                        if (!IsLoading) break;
                        await Task.Delay(10);
                    }

                } while (!IsServiceAvailable || IsLoading);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
