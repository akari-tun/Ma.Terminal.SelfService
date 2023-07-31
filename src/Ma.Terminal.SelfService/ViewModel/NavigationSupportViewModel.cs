using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Ma.Terminal.SelfService.ViewModel
{
    public abstract class NavigationSupportViewModel : ViewModelBase, INavigationSupport
    {
        public List<IPageViewInterface> PageList { get; set; }
        public IPageViewInterface CurrentPageView { get; set; }
        public IViewModel CurrentPageViewModel { get; set; }
        public Frame PageContainer { get; set; }

        public virtual void InitNavigationData()
        {

        }

        public virtual void NavigationTo(IPageViewInterface page)
        {
            if (page != null)
            {
                CurrentPageView = page;
                PageContainer.Content = CurrentPageView;
                CurrentPageViewModel = CurrentPageView.ViewModel;

                if (page is IBackspaceSupportView backSupport)
                {
                    backSupport.NavigatedTo(Ioc.Default.GetRequiredService<UserModel>());
                }
            }
        }

        public override void Initialization()
        {

        }
    }
}
