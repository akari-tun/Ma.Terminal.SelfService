using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.WebApi.Entities
{
    public class Apdu
    {
        /// <summary>
        /// 必须 指令顺序
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 必须 卡⽚指令
        /// </summary>
        public string CApdu { get; set; }
        /// <summary>
        /// 必须 期望返回响应码
        /// </summary>
        public string Sws { get; set; }
    }
}
