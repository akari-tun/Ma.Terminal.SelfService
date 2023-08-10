using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.Utils;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        ImageSource _fontImage;
        public ImageSource FontImage
        {
            get { return _fontImage; }
            set
            {
                SetProperty(ref _fontImage, value);
            }
        }

        ImageSource _backImage;
        public ImageSource BackImage
        {
            get { return _backImage; }
            set
            {
                SetProperty(ref _backImage, value);
            }
        }

        public override void Initialization()
        {
            Title = GetString("ConfirmInfo");

            var model = Ioc.Default.GetRequiredService<UserModel>();

            SetProperty(ref _name, model.UserName, nameof(Name));
            SetProperty(ref _idNo, model.IdCard, nameof(IdNo));
            SetProperty(ref _phoneNo, model.PhoneNumber, nameof(PhoneNo));
            SetProperty(ref _enterprise, model.CompanyName, nameof(Enterprise));

            var facePath = string.IsNullOrEmpty(model.CardFacePath) ? "pack://SiteOfOrigin:,,,/Resource/Image/Photo.png" : model.CardFacePath;
            var backPath = string.IsNullOrEmpty(model.CardFacePath) ? "pack://SiteOfOrigin:,,,/Resource/Image/Photo.png" : model.CardBackPath;

            FontImage = BitmapFrame.Create(new Uri(facePath), BitmapCreateOptions.None, BitmapCacheOption.Default);
            BackImage = BitmapFrame.Create(new Uri(backPath), BitmapCreateOptions.None, BitmapCacheOption.Default);
        }
    }
}
