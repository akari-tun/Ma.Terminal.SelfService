using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Model
{
    public class UserModel : IModel
    {
        /// <summary>
        /// 取卡类型
        /// </summary>
        public string PinkupType { get; set; }
        /// <summary>
        /// ⽤户⼿机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 取卡码
        /// </summary>
        public string PinkupCode { get; set; }
        /// <summary>
        /// ⽤户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 订单类型 1开卡 22补卡
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// ⽤户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 所属企业ID
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 所属企业名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 卡⽚正⾯地址
        /// </summary>
        public string CardFacePath { get; set; }
        /// <summary>
        /// 卡⽚背⾯地址
        /// </summary>
        public string CardBackPath { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }

        public void Update(UserModel model)
        {
            PhoneNumber = model.PhoneNumber;
            PinkupCode = model.PinkupCode;
            UserId = model.UserId;
            OrderType = model.OrderType;
            UserName = model.UserName;
            IdCard = model.IdCard;
            CompanyId = model.CompanyId;
            CompanyName = model.CompanyName;
            CardFacePath = model.CardFacePath;
            CardBackPath = model.CardBackPath;
            OrderId = model.OrderId;
        }
    }
}
