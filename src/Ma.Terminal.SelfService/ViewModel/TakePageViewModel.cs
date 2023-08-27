using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class TakePageViewModel : ViewModelBase
    {
        public Action<IPageViewInterface> NavigationTo;

        public override void Initialization()
        {
            Title = GetString("WaitCard");
            IsAllowBack = false;
        }
    }
}
