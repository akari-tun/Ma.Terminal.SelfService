using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.WebApi.Entities
{
    public class ApiRespone<T>
    {
        /// <summary>
        /// 0 表示成功
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 兼容code
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 兼容code
        /// </summary>
        public int Status { get; set; }
        public T Data { get; set; }
    }
}
