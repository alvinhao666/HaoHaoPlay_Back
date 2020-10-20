using System;
using System.Collections.Generic;

namespace Hao.EventData
{
    public class LogoutForUpdateAuthEventData
    {
        public List<long> UserIds { get; set; }

        public DateTime TimeNow { get; set; } = DateTime.Now;
    }
}
