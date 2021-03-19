using System;

namespace Hao.Runtime
{
    /// <summary>
    /// 事件总线传输对象
    /// </summary>
    public abstract class H_EventData
    {
        /// <summary>
        /// 事件发布者
        /// </summary>
        public CurrentUser PublishUser { get; set; } //不能使用接口，接口无法json序列化

        /// <summary>
        /// 事件发布对象
        /// </summary>
        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}