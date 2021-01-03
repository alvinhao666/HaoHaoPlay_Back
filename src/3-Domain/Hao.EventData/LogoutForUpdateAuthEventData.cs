using System;
using System.Collections.Generic;
using Hao.Runtime;

namespace Hao.EventData
{
    public class LogoutForUpdateAuthEventData: H_EventData
    {
        public List<long> UserIds { get; set; }
    }
}
