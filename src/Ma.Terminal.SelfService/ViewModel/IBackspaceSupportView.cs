using Ma.Terminal.SelfService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public interface IBackspaceSupportView
    {
        IPageViewInterface BackPageView { get; set; }

        void NavigatedTo(IModel model);
    }
}
