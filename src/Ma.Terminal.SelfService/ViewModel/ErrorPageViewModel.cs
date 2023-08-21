using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class ErrorPageViewModel : ViewModelBase
    {
        public Action<IPageViewInterface> NavigationTo;

        public string ErrorType { get; set; }

        string _errMsg;
        public string ErrMsg
        {
            get { return _errMsg; }
            set
            {
                SetProperty(ref _errMsg, value);
            }
        }

        public override void Initialization()
        {
            Title = GetString("Error");
        }
    }
}
