using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.EventData
{
    public class LogoutEventData
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public List<long> UserIds { get; set; }

        public DateTime TimeNow { get; set; } = DateTime.Now;
    }
}
