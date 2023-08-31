using Ma.Terminal.SelfService.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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
        [JsonConverter(typeof(JsonStringToIntConverter))]
        public int Status { get; set; }
        /// <summary>
        /// 兼容code
        /// </summary>
        public string Desc { get; set; }
        public T Data { get; set; }
    }
}
