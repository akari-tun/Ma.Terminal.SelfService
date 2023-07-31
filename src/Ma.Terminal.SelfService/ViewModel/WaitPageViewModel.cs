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

namespace Ma.Terminal.SelfService.ViewModel
{
    public class WaitPageViewModel : ViewModelBase
    {
        private Device.Printer.Operator _printer;
        private Device.Reader.Operator _reader;
        private Requester _api;

        public delegate void CardPrintedHandler(bool isSuccess, string msg);
        public event CardPrintedHandler OnCardPrinted;

        public Action<IPageViewInterface> NavigationTo;

        public WaitPageViewModel(Requester api, Device.Reader.Operator reader, Device.Printer.Operator printer)
        {
            _api = api;
            _reader = reader;
            _printer = printer;
        }

        public override void Initialization()
        {
            Title = GetString("TakeCard");
        }

        public void PrintCard()
        {
            Task.Run(async () =>
            {
                if (!_printer.MoveToRfPosition())
                {
                    OnCardPrinted?.Invoke(false, _printer.LastError);
                }

                if (!_reader.OpenCard(out byte[] uid))
                {
                    OnCardPrinted?.Invoke(false, _reader.LastError);
                }

                var model = Ioc.Default.GetRequiredService<UserModel>();

                var openCardApdu = await _api.OpenCardApdu(model.OrderId,
                    model.UserId,
                    FunTools.BytesToHexStr(uid));

                if (openCardApdu == null)
                {
                    OnCardPrinted?.Invoke(false, _reader.LastError);
                }

                if (!_reader.ExecuteApdu(FunTools.StrToHexBytes(openCardApdu.CApdus.CApdu),
                        out byte[] rsp))
                {
                    OnCardPrinted?.Invoke(false, _reader.LastError);
                }

                var apduExe = await _api.ApduExeResult(openCardApdu.CApdus.Index.ToString(),
                    FunTools.BytesToHexStr(rsp),
                    "0x00",
                    model.UserId,
                    FunTools.BytesToHexStr(uid));

                if (apduExe == null)
                {
                    OnCardPrinted?.Invoke(false, _reader.LastError);
                }

                foreach (var item in apduExe.CApdus)
                {
                    byte[] tmp;
                    if (!_reader.ExecuteApdu(FunTools.StrToHexBytes(item.CApdu),
                            out tmp))
                    {
                        OnCardPrinted?.Invoke(false, _reader.LastError);
                    }
                }

                var facePath = string.IsNullOrEmpty(model.CardFacePath) ? "pack://SiteOfOrigin:,,,/Resource/Image/Photo.png" : model.CardFacePath;
                var backPath = string.IsNullOrEmpty(model.CardFacePath) ? "pack://SiteOfOrigin:,,,/Resource/Image/Photo.png" : model.CardBackPath;

                var fontImage = BitmapFrame.Create(new Uri(facePath), BitmapCreateOptions.None, BitmapCacheOption.Default);
                var backImage = BitmapFrame.Create(new Uri(backPath), BitmapCreateOptions.None, BitmapCacheOption.Default);

                OnCardPrinted?.Invoke(true, "制卡成功");
            });
        }
    }
}
