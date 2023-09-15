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
using System.Windows;

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
        private int _timeOut = 120;

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

        string _processMsg = string.Empty;
        public string ProcessMsg
        {
            get => _processMsg;
            set
            {
                SetProperty(ref _processMsg, value);
            }
        }

        public bool IsWaiting { get; set; }

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
            ProcessMsg = string.Empty;
            Timeout = _machine.PrinteTimeout;

            var model = Ioc.Default.GetRequiredService<UserModel>();
            _issueCardModel = new IssueCardModel
            {
                OrderId = model.OrderId,
                UserId = model.UserId
            };

            _logger.Trace($"开始制卡 -> [OrderId:{model.OrderId}] [UserId:{model.UserId}]");

            Task.Run(async () =>
            {
                IsWaiting = true;

                while (IsWaiting)
                {
                    Timeout -= 1;
                    IsWaiting = Timeout >= 0;
                    await Task.Delay(1000);
                }

                _printer.IsWaiting = false;
            });

            Task.Run(async () =>
            {
                bool isCardReady = false;
                int retry = 5;

                try
                {
                    ProcessMsg = "移动卡片到RF位置";
                    _logger.Trace($"制卡中 -> {ProcessMsg}");

                    while (!isCardReady && retry > 0)
                    {
                        isCardReady = _printer.MoveToRfPosition();
                        await Task.Delay(300);
                        retry--;
                    }

                    if (!isCardReady)
                    {
                        _logger.Trace($"制失败 -> {_printer.LastError}");
                        OnCardPrinted?.Invoke(false, _printer.LastError);
                        return;
                    }

                    _config.Card--;

                    ProcessMsg = "卡片上电";
                    _logger.Trace($"制卡中 -> {ProcessMsg}");

                    if (!_reader.OpenCard(out _uid))
                    {
                        _logger.Trace($"制失败 -> {_reader.LastError}");
                        OnCardPrinted?.Invoke(false, _reader.LastError);
                        _printer.ExitCard();
                        _config.Save();
                        return;
                    }

                    _issueCardModel.Uid = _uid;

                    ProcessMsg = "请求发卡指令";
                    _logger.Trace($"制卡中 -> {ProcessMsg}");

                    var openCardApdu = await _api.OpenCardApdu(model.OrderId,
                        model.UserId,
                        FunTools.BytesToHexStr(_uid));

                    if (openCardApdu == null)
                    {
                        _logger.Trace($"制失败 -> {_api.LastMessage}");
                        OnCardPrinted?.Invoke(false, _api.LastMessage);
                        _printer.ExitCard();
                        _config.Save();
                        return;
                    }

                    int lastIndex = 0;
                    byte[] lastRsp = null;

                    ProcessMsg = "执行发卡指令";
                    _logger.Trace($"制卡中 -> {ProcessMsg}");

                    foreach (var item in openCardApdu.Capdus)
                    {
                        ProcessMsg = $"执行发卡指令 [{item.Index}]";
                        _logger.Trace($"制卡中 -> {ProcessMsg} {item.Capdu}");

                        if (!_reader.ExecuteApdu(FunTools.StrToHexBytes(item.Capdu),
                                out lastRsp,
                                item.Sws))
                        {
                            _logger.Trace($"制失败 -> {_reader.LastError}");
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
                        ProcessMsg = $"请求下一条发卡指令";
                        _logger.Trace($"制卡中 -> {ProcessMsg} {FunTools.BytesToHexStr(lastRsp)}");

                        var apduExe = await _api.ApduExeResult(lastIndex.ToString(),
                            FunTools.BytesToHexStr(lastRsp),
                            result,
                            model.UserId,
                            FunTools.BytesToHexStr(_uid));

                        if (apduExe == null)
                        {
                            isHasNext = false;
                            _logger.Trace($"制失败 -> {_api.LastMessage}");
                            OnCardPrinted?.Invoke(false, _api.LastMessage);
                            _printer.ExitCard();
                            _config.Save();
                            return;
                        }

                        if (apduExe.Capdus != null && apduExe.Capdus.Count > 0)
                        {
                            foreach (var item in apduExe.Capdus)
                            {
                                ProcessMsg = $"执行发卡指令 [{item.Index}]";
                                _logger.Trace($"制卡中 -> {ProcessMsg} {item.Capdu}");

                                lastIndex = item.Index;

                                if (!_reader.ExecuteApdu(FunTools.StrToHexBytes(item.Capdu),
                                        out lastRsp,
                                        item.Sws))
                                {
                                    ProcessMsg = $"指令执行失败 [{item.Index}]";
                                    _logger.Trace($"制卡中 -> {ProcessMsg}");
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

                    ProcessMsg = $"打印卡片";
                    _logger.Trace($"制卡中 -> {ProcessMsg}");

                    _doucument.Print();
                }
                catch (Exception ex)
                {
                    ProcessMsg = $"制卡失败：{ex.Message}";

                    OnCardPrinted?.Invoke(false, "请联系系统管理员");
                    _printer.ExitCard();

                    _logger.Trace(ex.Message);
                    _logger.Error(ex.StackTrace);
                }

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

        private void PrintEnd(object sender, PrintEventArgs e)
        {
            try
            {
                _config.Ink--;
                _config.Lanyard--;

                _config.Save();

                _finishCards.Enqueue(_issueCardModel);

                WaitingPrinte();
                if (!_isLoading) RunUpload();
            }
            catch (Exception ex)
            {
                OnCardPrinted?.Invoke(false, ex.Message);
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
                    ProcessMsg = "等待打印";
                    _logger.Trace($"制卡中 -> {ProcessMsg}");

                    int waitTime = 20;
                    while (waitTime > 0)
                    {
                        ProcessMsg = $"打印机预热，等待打印开始 {waitTime}";
                        await Task.Delay(1000);
                        waitTime -= 1;
                    }

                    ProcessMsg = $"打印中，请等待。。。";

                    var result = await _printer.WaitPrintEnd(Timeout * 1000);
                    ProcessMsg = result ? "制卡成功" : "等待打印超时";
                    _logger.Trace($"制卡完成 -> {ProcessMsg}");

                    OnCardPrinted?.Invoke(true, ProcessMsg);

                    var model = Ioc.Default.GetRequiredService<UserModel>();

                    await Task.Run(() => _lanyard.RollLanyard(_machine.MaxLanyard - _config.Lanyard, model.OrderId));
                    await Task.Run(() => _light.Light());
                    await Task.Run(() => _light.Light(_machine.MaxLanyard - _config.Lanyard));
                }
                catch (Exception ex)
                {
                    OnCardPrinted?.Invoke(false, ex.Message);
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
