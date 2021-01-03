using System;

namespace Hao.Runtime
{
    public abstract class H_EventData
    {
        public CurrentUser CurrentUser { get; set; }
        
        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}