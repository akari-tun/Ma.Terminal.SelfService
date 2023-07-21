using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class WaitPageViewModel : ViewModelBase
    {
        public delegate void CardPrintedHandler();

        public event CardPrintedHandler OnCardPrinted;

        public Action<IPageViewInterface> NavigationTo;

        public override void Initialization()
        {
            Title = GetString("TakeCard");
        }

        public void PrintCard()
        {
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                OnCardPrinted?.Invoke();
            });
        }
    }
}
