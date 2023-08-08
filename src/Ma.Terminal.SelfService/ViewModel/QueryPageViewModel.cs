using Ma.Terminal.SelfService.Model;
using Ma.Terminal.SelfService.WebApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfService.ViewModel
{
    public class QueryPageViewModel : ViewModelBase
    {
        private Requester _api;

        public Action<IPageViewInterface> NavigationTo;

        public QueryPageViewModel(Requester api)
        {
            _api = api;
        }

        public override void Initialization()
        {
            Title = GetString("Query");
        }

        public async Task<UserModel> Query(string phone, string code)
        {
            var entity = await _api.OpenCardUserDetail(phone, code);

            if (entity != null)
            {
                return new UserModel()
                {
                    PinkupPhoneNumber = phone,
                    PhoneNumber = entity.PhoneNumber,
                    PinkupCode = code,
                    UserId = entity.UserId,
                    OrderType = entity.OrderType,
                    UserName = entity.UserName,
                    IdCard = entity.IdCard,
                    CompanyId = entity.CompanyId,
                    CompanyName = entity.CompanyName,
                    CardFacePath = entity.CardFacePath,
                    CardBackPath = entity.CardBackPath,
                    OrderId = entity.OrderId
                };
            }

            return null;
        }
    }
}
