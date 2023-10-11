using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class TakePageViewModel : ViewModelBase
    {
        public Action<IPageViewInterface> NavigationTo;

        public int Timeout { get; set; }

        public bool IsWaiting { get; set; }

        public string Close => Timeout >= 0 ? $"{GetString("Close")} {Timeout}" : GetString("Close");

        public override void Initialization()
        {
            Title = GetString("TakeCard");
            IsAllowBack = false;
        }

        public void WaitAutoClose(IPageViewInterface page)
        {
            if (!IsWaiting)
            {
                IsWaiting = true;
                Timeout = 60;
                OnPropertyChanged(nameof(Close));

                Task.Run(() =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    while (IsWaiting)
                    {
                        if (stopwatch.ElapsedMilliseconds > 1000)
                        {
                            stopwatch.Restart();

                            Timeout -= 1;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                OnPropertyChanged(nameof(Close));
                                if (Timeout < 1)
                                {
                                    NavigationTo(page);
                                    IsWaiting = false;
                                }
                            }));
                        }

                        Thread.Sleep(0);
                    }
                });
            }
        }
    }
}
