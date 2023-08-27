using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class MainContainerViewModel : NavigationSupportViewModel
    {
        private Requester _api;
        private Machine _machine;
        Logger _logger = LogManager.GetCurrentClassLogger();

        private bool _isCheckRunning = false;
        public bool IsCheckRunning
        {
            get => _isCheckRunning;
            set
            {
                SetProperty(ref _isCheckRunning, value);
            }
        }

        public MainContainerViewModel(Machine machine, Requester api) : base()
        {
            _machine = machine;
            _api = api;
        }

        public async Task CheckStatus()
        {
            try
            {
                do
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
                    }
                    else
                    {
                        _machine.Detail.Status = 0;
                        _logger.Info($"GetMachineDetail => {_api.LastMessage}");
                    }


                    for (int i = 0; i < 360000; i++)
                    {
                        if (!_isCheckRunning) break;
                        await Task.Delay(10);
                    }

                } while (_isCheckRunning);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
            finally
            {
                _isCheckRunning = false;
            }
        }
    }
}
