using System;
using System.Collections.Generic;
using Hao.Runtime;

namespace Hao.EventData
{
    public class LogoutEventData : H_EventData
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public List<long> UserIds { get; set; }
    }
}
