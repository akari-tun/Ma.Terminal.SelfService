using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfService.WebApi.Entities
{
    public class OpenCardApdu
    {
        /// <summary>
        /// ⾮必须 ⽤户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// APDU
        /// </summary>
        public Apdu CApdus { get; set; }
    }
}
