using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class ConfirmPageViewModel : ViewModelBase
    {
        public Action<IPageViewInterface> NavigationTo;

        string _name;
        public string Name
        {
            get { return _name; }
            set 
            {
                SetProperty(ref _name, value);
            }
        }

        string _idNo;
        public string IdNo
        {
            get { return _idNo; }
            set
            {
                SetProperty(ref _idNo, value);
            }
        }

        string _phoneNo;
        public string PhoneNo
        {
            get { return _phoneNo; }
            set
            {
                SetProperty(ref _phoneNo, value);
            }
        }

        string _enterprise;
        public string Enterprise
        {
            get { return _enterprise; }
            set
            {
                SetProperty(ref _enterprise, value);
            }
        }

        public override void Initialization()
        {
            Title = GetString("ConfirmInfo");
        }
    }
}
