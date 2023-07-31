using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class ErrorPageViewModel : ViewModelBase
    {
        public Action<IPageViewInterface> NavigationTo;

        public string ErrorType { get; set; }

        public override void Initialization()
        {
            Title = GetString("Query");
        }
    }
}
