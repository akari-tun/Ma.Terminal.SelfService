using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public interface IErrorPageSupportView
    {
        IPageViewInterface ErrorPageView { get; set; }
    }
}
