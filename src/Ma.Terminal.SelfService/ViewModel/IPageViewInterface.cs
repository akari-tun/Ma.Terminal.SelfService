using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public interface IPageViewInterface
    {
        IViewModel ViewModel { get; }
        IPageViewInterface Init(INavigationSupport navigationParent);
    }
}
