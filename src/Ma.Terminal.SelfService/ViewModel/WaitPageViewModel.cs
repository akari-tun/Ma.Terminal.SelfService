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

namespace Ma.Terminal.SelfService.ViewModel
{
    public class WaitPageViewModel : ViewModelBase
    {
        private PrintDocument _doucument; 
        private Device.Printer.Operator _printer;
        private Device.Reader.Operator _reader;
        private Device.Lanyard.Operator _lanyard;
        private Device.Light.Operator _light;
        private Requester _api;
        private Machine _machine;
        private byte[] _uid;
        private Queue<Image> _waitPrintImages;

        public delegate void CardPrintedHandler(bool isSuccess, string msg);
        public event CardPrintedHandler OnCardPrinted;

        public Action<IPageViewInterface> NavigationTo;

        string _errMsg;
        public string ErrMsg
        {
            get { return _errMsg; }
            set
            {
                SetProperty(ref _errMsg, value);
            }
        }

        public WaitPageViewModel(Machine machine,
            Requester api,
            Device.Reader.Operator reader,
            Device.Printer.Operator printer,
            Device.Lanyard.Operator lanyard,
            Device.Light.Operator light)
        {
            _machine = machine;
            _api = api;
            _reader = reader;
            _printer = printer;
            _lanyard = lanyard;
            _light = light;

            _doucument = new PrintDocument();
            _doucument.PrinterSettings.PrinterName = _machine.PrinterName;
            _doucument.PrintPage += PrintPage;
            _doucument.EndPrint += PrintEnd;

            _waitPrintImages = new Queue<Image>();
        }

        public override void Initialization()
        {
            Title = GetString("TakeCard");
        }

        public void PrintCard()
        {
            Task.Run(async () =>
            {
                var model = Ioc.Default.GetRequiredService<UserModel>();

                if (!_printer.MoveToRfPosition())
                {
                    OnCardPrinted?.Invoke(false, _printer.LastError);
                    return;
                }

                if (!_reader.OpenCard(out _uid))
                {
                    OnCardPrinted?.Invoke(false, _reader.LastError);
                    return;
                }

                var openCardApdu = await _api.OpenCardApdu(model.OrderId,
                    model.UserId,
                    FunTools.BytesToHexStr(_uid));

                if (openCardApdu == null)
                {
                    OnCardPrinted?.Invoke(false, _api.LastMessage);
                    return;
                }

                int lastIndex = 0;
                byte[] lastRsp = null;

                foreach (var item in openCardApdu.Capdus)
                {
                    Debug.WriteLine($"Exec apdu:{item.Capdu} index:[{item.Index}]");
                    if (!_reader.ExecuteApdu(FunTools.StrToHexBytes(item.Capdu),
                            out lastRsp,
                            item.Sws))
                    {
                        OnCardPrinted?.Invoke(false, _reader.LastError);
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
                        return;
                    }

                    if (apduExe.Capdus != null && apduExe.Capdus.Count > 0)
                    {
                        foreach (var item in apduExe.Capdus)
                        {
                            lastIndex = item.Index;
                            Debug.WriteLine($"apdu:{item.Capdu} index:[{item.Index}]");
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

                var facePath = string.IsNullOrEmpty(model.CardFacePath) ? "pack://SiteOfOrigin:,,,/Resource/Image/Photo.png" : model.CardFacePath;
                var backPath = string.IsNullOrEmpty(model.CardBackPath) ? "pack://SiteOfOrigin:,,,/Resource/Image/Photo.png" : model.CardBackPath;

                var fontImage = BitmapFrame.Create(new Uri(facePath), BitmapCreateOptions.None, BitmapCacheOption.Default);
                var backImage = BitmapFrame.Create(new Uri(backPath), BitmapCreateOptions.None, BitmapCacheOption.Default);

                Image front = null;
                Image back = null;

                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(fontImage);
                    enc.Save(outStream);
                    front = new Bitmap(outStream);
                    front.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    front.Save($"{AppDomain.CurrentDomain.BaseDirectory}Resource\\Front.jpg");
                }

                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(backImage);
                    enc.Save(outStream);
                    back = new Bitmap(outStream);
                    back.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    back.Save($"{AppDomain.CurrentDomain.BaseDirectory}Resource\\Back.jpg");
                }

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
            var model = Ioc.Default.GetRequiredService<UserModel>();

            var result = await _api.Finish(model.OrderId,
                model.UserId,
                FunTools.BytesToHexStr(_uid),
                _machine.MachineNo);

            if (result == null)
            {
                OnCardPrinted?.Invoke(false, _api.LastMessage);
                return;
            }
            
            int.TryParse(_machine.Detail.CardCount, out int card);
            int.TryParse(_machine.Detail.InkCount, out int ink);
            int.TryParse(_machine.Detail.CardRopeCover, out int lanyard);

            card--;
            ink--;
            lanyard--;

            await Task.Run(() => _lanyard.RollLanyard(_machine.MaxLanyard - lanyard, model.OrderId));
            await Task.Run(() => _light.Light(_machine.MaxLanyard - lanyard));

            await _api.SaveMachine(_machine.MachineNo,
                _machine.Detail.CardCount,
                _machine.Detail.InkCount,
                _machine.Detail.CardRopeCover);

            OnCardPrinted?.Invoke(true, "制卡成功");
        }
    }
}
