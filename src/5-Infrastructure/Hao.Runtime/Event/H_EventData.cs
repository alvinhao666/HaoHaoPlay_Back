using System;

namespace Hao.Runtime
{
    /// <summary>
    /// �¼�����ʵ�����
    /// </summary>
    public abstract class H_EventData
    {
        public CurrentUser PublishUser { get; set; }
        
        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}