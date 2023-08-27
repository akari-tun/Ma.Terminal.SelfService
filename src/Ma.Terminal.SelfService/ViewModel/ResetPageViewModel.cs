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

        int _cardSurplus;
        public int CardSurplus
        {
            get { return _cardSurplus; }
            set
            {
                SetProperty(ref _cardSurplus, value);
            }
        }

        int _inkSurplus;
        public int InkSurplus
        {
            get { return _inkSurplus; }
            set
            {
                SetProperty(ref _inkSurplus, value);
            }
        }

        int _lanyardSurplus;
        public int LanyardSurplus
        {
            get { return _lanyardSurplus; }
            set
            {
                SetProperty(ref _lanyardSurplus, value);
            }
        }

        int _maxCardValue;

        public int MaxCardValue
        {
            get { return _maxCardValue; }
            set
            {
                SetProperty(ref _maxCardValue, value);
            }
        }

        int _maxInkValue;
        public int MaxInkValue
        {
            get { return _maxInkValue; }
            set
            {
                SetProperty(ref _maxInkValue, value);
            }
        }

        int _maxLanyardValue;
        public int MaxLanyardValue
        {
            get { return _maxLanyardValue; }
            set
            {
                SetProperty(ref _maxLanyardValue, value);
            }
        }

        public override void Initialization()
        {
            Title = GetString("Config");
            IsAllowBack = true;

            var machine = Ioc.Default.GetRequiredService<Machine>();

            MaxCardValue = machine.MaxCard;
            MaxInkValue = machine.MaxInk;
            MaxLanyardValue = machine.MaxLanyard;

            int.TryParse(machine.Detail.CardCount, out _cardSurplus);
            int.TryParse(machine.Detail.InkCount, out _inkSurplus);
            int.TryParse(machine.Detail.CardRopeCover, out _lanyardSurplus);

            OnPropertyChanged(nameof(CardSurplus));
            OnPropertyChanged(nameof(InkSurplus));
            OnPropertyChanged(nameof(LanyardSurplus));
        }
    }
}
