using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public interface INextPageSupportView
    {
        IPageViewInterface NextPageView { get; set; }
    }
}
