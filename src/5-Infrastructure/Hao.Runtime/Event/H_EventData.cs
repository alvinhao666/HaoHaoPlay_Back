using System;

namespace Hao.Runtime
{
    /// <summary>
    /// 事件传输实体基类
    /// </summary>
    public abstract class H_EventData
    {
        public CurrentUser PublishUser { get; set; }
        
        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}