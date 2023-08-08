using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class ResetPageViewModel : ViewModelBase
    {
        public Action<IPageViewInterface> NavigationTo;

        string _cardSurplus;
        public string CardSurplus
        {
            get { return _cardSurplus; }
            set
            {
                SetProperty(ref _cardSurplus, value);
            }
        }

        string _inkSurplus;
        public string InkSurplus
        {
            get { return _inkSurplus; }
            set
            {
                SetProperty(ref _inkSurplus, value);
            }
        }

        string _lanyardSurplus;
        public string LanyardSurplus
        {
            get { return _lanyardSurplus; }
            set
            {
                SetProperty(ref _lanyardSurplus, value);
            }
        }

        public override void Initialization()
        {
            Title = GetString("Reset");

            var machine = Ioc.Default.GetRequiredService<Machine>();

            SetProperty(ref _cardSurplus, machine.Detail.CardCount, nameof(CardSurplus));
            SetProperty(ref _inkSurplus, machine.Detail.InkCount, nameof(InkSurplus));
            SetProperty(ref _lanyardSurplus, machine.Detail.CardRopeCover, nameof(LanyardSurplus));
        }
    }
}
