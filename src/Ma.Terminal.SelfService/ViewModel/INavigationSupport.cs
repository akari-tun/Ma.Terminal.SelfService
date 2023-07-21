using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Ma.Terminal.SelfService.ViewModel
{
    public interface INavigationSupport
    {
        List<IPageViewInterface> PageList { get; set; }
        IPageViewInterface CurrentPageView { get; set; }
        IViewModel CurrentPageViewModel { get; set; }
        Frame PageContainer { get; set; }

        void NavigationTo(IPageViewInterface page);
        void InitNavigationData();
    }
}
