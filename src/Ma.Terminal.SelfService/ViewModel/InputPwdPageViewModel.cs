using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class InputPwdPageViewModel : ViewModelBase
    {
        public Action<IPageViewInterface> NavigationTo;

        public override void Initialization()
        {
            Title = GetString("CheckPwd");
        }
    }
}
