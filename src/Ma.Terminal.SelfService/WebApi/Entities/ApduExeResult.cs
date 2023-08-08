using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.WebApi.Entities
{
    public class ApduExeResult
    {
        /// <summary>
        /// 必须 ⼀卡通⽤户标识
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 必须 个⼈化是否完成
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// APDUs
        /// </summary>
        public List<Apdu> Capdus { get; set; }
    }
}
