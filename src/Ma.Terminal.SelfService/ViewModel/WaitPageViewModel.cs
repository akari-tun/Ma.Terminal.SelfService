using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Device.Reader;
using Ma.Terminal.SelfService.Device.Printer;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.WebApi;
using Ma.Terminal.SelfService.WebApi.Entities;
using Ma.Terminal.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using Ma.Terminal.SelfService.Utils;
using NLog;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class WaitPageViewModel : ViewModelBase
    {
        private PrintDocument _doucument; 
        private Device.Printer.Operator _printer;
        private Device.Reader.Operator _reader;
        private Device.Lanyard.Operator _lanyard;
        private Device.Light.Operator _light;
        private ItemsConfig _config;
        private Requester _api;
        private Machine _machine;
        private byte[] _uid;
        private IssueCardModel _issueCardModel;
        private Queue<Image> _waitPrintImages;
        private Queue<IssueCardModel> _finishCards;
        private bool _isLoading = false;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public delegate void CardPrintedHandler(bool isSuccess, string msg);
        public event CardPrintedHandler OnCardPrinted;

        public Action<IPageViewInterface> NavigationTo;

        int _timeout;
        public int Timeout
        {
            get => _timeout;
            set
            {
                SetProperty(ref _timeout, value);
            }
        }

        public WaitPageViewModel(Machine machine,
            Requester api,
            Device.Reader.Operator reader,
            Device.Printer.Operator printer,
            Device.Lanyard.Operator lanyard,
            Device.Light.Operator light,
            ItemsConfig config) : base()
        {
            _machine = machine;
            _api = api;
            _reader = reader;
            _printer = printer;
            _lanyard = lanyard;
            _light = light;
            _config = config;

            _doucument = new PrintDocument();
            _doucument.PrinterSettings.PrinterName = _machine.PrinterName;
            _doucument.PrintPage += PrintPage;
            _doucument.EndPrint += PrintEnd;

            _waitPrintImages = new Queue<Image>();

            _finishCards = new Queue<IssueCardModel>();
        }

        public override void Initialization()
        {
            Title = GetString("WaitCard");
            IsAllowBack = false;
        }

        public void PrintCard()
        {
            _issueCardModel = new IssueCardModel();

            Task.Run(() => _light.Light());

            Task.Run(async () =>
            {
                var model = Ioc.Default.GetRequiredService<UserModel>();

                _issueCardModel.OrderId = model.OrderId;
                _issueCardModel.UserId = model.UserId;

                bool isCardReady = false;
                int retry = 5;

                while (!isCardReady && retry > 0)
                {
                    isCardReady = _printer.MoveToRfPosition();
                    await Task.Delay(300);
                    retry--;
                }

                if (!isCardReady)
                {
                    OnCardPrinted?.Invoke(false, _printer.LastError);
                    return;
                }

                _config.Card--;

                if (!_reader.OpenCard(out _uid))
                {
                    OnCardPrinted?.Invoke(false, _reader.LastError);
                    _printer.ExitCard();
                    _config.Save();
                    return;
                }

                _issueCardModel.Uid = _uid;

                var openCardApdu = await _api.OpenCardApdu(model.OrderId,
                    model.UserId,
                    FunTools.BytesToHexStr(_uid));

                if (openCardApdu == null)
                {
                    OnCardPrinted?.Invoke(false, _api.LastMessage);
                    _printer.ExitCard();
                    _config.Save();
                    return;
                }

                int lastIndex = 0;
                byte[] lastRsp = null;

                foreach (var item in openCardApdu.Capdus)
                {
                    if (!_reader.ExecuteApdu(FunTools.StrToHexBytes(item.Capdu),
                            out lastRsp,
                            item.Sws))
                    {
                        OnCardPrinted?.Invoke(false, _reader.LastError);
                        _printer.ExitCard();
                        _config.Save();
                        return;
                    }

                    lastIndex = item.Index;
                }

                bool isHasNext = true;
                int result = 0;

                while (isHasNext)
                {
                    var apduExe = await _api.ApduExeResult(lastIndex.ToString(),
                        FunTools.BytesToHexStr(lastRsp),
                        result,
                        model.UserId,
                        FunTools.BytesToHexStr(_uid));

                    if (apduExe == null)
                    {
                        isHasNext = false;
                        OnCardPrinted?.Invoke(false, _api.LastMessage);
                        _printer.ExitCard();
                        _config.Save();
                        return;
                    }

                    if (apduExe.Capdus != null && apduExe.Capdus.Count > 0)
                    {
                        foreach (var item in apduExe.Capdus)
                        {
                            lastIndex = item.Index;

                            if (!_reader.ExecuteApdu(FunTools.StrToHexBytes(item.Capdu),
                                    out lastRsp,
                                    item.Sws))
                            {
                                result = 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        isHasNext = !apduExe.Finished;
                    }
                }

                _waitPrintImages.Clear();

                var front = string.IsNullOrEmpty(model.CardFacePath) ? Image.FromFile($"{AppDomain.CurrentDomain.BaseDirectory}Resource\\Image\\Photo.png") : ImageUtils.GetBitmapFromUrl(model.CardFacePath);
                var back = string.IsNullOrEmpty(model.CardBackPath) ? Image.FromFile($"{AppDomain.CurrentDomain.BaseDirectory}Resource\\Image\\Photo.png") : ImageUtils.GetBitmapFromUrl(model.CardBackPath);

                front.Save($"{AppDomain.CurrentDomain.BaseDirectory}Resource\\Front.jpg");
                back.Save($"{AppDomain.CurrentDomain.BaseDirectory}Resource\\Back.jpg");

                front.RotateFlip(RotateFlipType.Rotate90FlipNone);
                back.RotateFlip(RotateFlipType.Rotate270FlipNone);

                _waitPrintImages.Enqueue(front);
                _waitPrintImages.Enqueue(back);

                _doucument.Print();
            });
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_waitPrintImages.Count > 0)
            {
                var img = _waitPrintImages.Dequeue();

                Rectangle rect;
                IntPtr hPrinterDC;

                e.Graphics.PageUnit = GraphicsUnit.Pixel;
                hPrinterDC = e.Graphics.GetHdc();
                e.Graphics.ReleaseHdc(hPrinterDC);

                rect = new Rectangle(0, 0, 1026, 655);
                e.Graphics.DrawImage(img, rect);

                img.Dispose();
            }

            e.HasMorePages = _waitPrintImages.Count > 0;
        }

        private async void PrintEnd(object sender, PrintEventArgs e)
        {
            try
            {
                var model = Ioc.Default.GetRequiredService<UserModel>();

                await Task.Run(() => _lanyard.RollLanyard(_machine.MaxLanyard - _config.Lanyard, model.OrderId));
                await Task.Run(() => _light.Light(_machine.MaxLanyard - _config.Lanyard));

                _config.Ink--;
                _config.Lanyard--;

                _config.Save();

                _finishCards.Enqueue(_issueCardModel);

                WaitingPrinte();
                if (!_isLoading) RunUpload();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
        }

        private void WaitingPrinte()
        {
            Task.Run(async () =>
            {
                try
                {
                    OnCardPrinted?.Invoke(true, "等待打印");
                    var result = await _printer.WaitPrintEnd(60000, t => Timeout = t);
                    OnCardPrinted?.Invoke(true, result ? "制卡成功" : "等待打印超时");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);
                }
            });
        }

        private void RunUpload()
        {
            Task.Run(async () =>
            {
                _isLoading = true;

                while (_isLoading)
                {
                    try
                    {
                        IssueCardModel model = _finishCards.Dequeue();

                        if (model != null)
                        {
                            if (!await Finish(model))
                            {
                                _finishCards.Enqueue(model);
                            }
                        }

                        _isLoading = _finishCards.Count > 0;

                        for (int i = 0; i < 6000; i++)
                        {
                            if (!_isLoading) break;
                            await Task.Delay(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                        _logger.Error(ex.StackTrace);
                    }

                }
            });
        }

        private async Task<bool> Finish(IssueCardModel model)
        {
            if (await _api.Finish(model.OrderId,
                                  model.UserId,
                                  FunTools.BytesToHexStr(model.Uid),
                                  _machine.MachineNo))
            {
                _logger.Info($"/yktInfo/openCard/finish -> [OrderId:{model.OrderId}] [UserId:{model.UserId}] [Uid:{FunTools.BytesToHexStr(model.Uid)}] Success");

                return true;
            }
            else
            {
                _logger.Info($"/yktInfo/openCard/finish -> [OrderId:{model.OrderId}] [UserId:{model.UserId}] [Uid:{FunTools.BytesToHexStr(model.Uid)}] {_api.LastMessage}");
            }

            return false;
        }
    }
}
