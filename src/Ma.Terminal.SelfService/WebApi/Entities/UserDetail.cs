using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.WebApi.Entities
{
    public class UserDetail
    {
        /// <summary>
        /// 必须 ⽤户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 必须 订单类型 1开卡 22补卡
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 必须 ⽤户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 必须 证件号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 必须 ⽤户⼿机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 必须 所属企业ID
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 必须 所属企业名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 必须 卡⽚正⾯地址
        /// </summary>
        public string CardFacePath { get; set; }
        /// <summary>
        /// 必须 卡⽚背⾯地址
        /// </summary>
        public string CardBackPath { get; set; }
    }
}
