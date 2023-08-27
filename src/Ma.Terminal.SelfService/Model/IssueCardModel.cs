using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Model
{
    public class IssueCardModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// ⽤户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        public byte[] Uid { get; set; }
    }
}
