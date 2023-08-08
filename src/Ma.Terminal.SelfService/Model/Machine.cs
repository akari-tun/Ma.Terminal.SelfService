using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.Model
{
    public class Machine
    {
        public string MachineNo { get; set; }
        public string ApiUrl { get; set; }
        public string PrinterName { get; set; }
        public int MaxCard { get; set; }
        public int MaxLanyard { get; set; }
        public int MaxInk { get; set; }

        public Detail Detail { get; set; }
    }

    public class Detail
    {
        /// <summary>
        /// 必须 所属园区ID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 必须 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 必须 剩余卡⽚
        /// </summary>
        public string CardCount { get; set; }
        /// <summary>
        /// 必须 剩余墨⽔
        /// </summary>
        public string InkCount { get; set; }
        /// <summary>
        /// 必须 剩余卡绳卡套
        /// </summary>
        public string CardRopeCover { get; set; }
        /// <summary>
        /// 必须 状态（1：启⽤ 2：停⽤）
        /// </summary>
        public int Status { get; set; }
    }
}
