using System;

namespace Hao.Runtime
{
    /// <summary>
    /// 事件传输实体基类
    /// </summary>
    public abstract class H_EventData
    {
        /// <summary>
        /// 事件发布者
        /// </summary>
        public CurrentUser PublishUser { get; set; } //只能用类，接口和抽象类反序列化会报错

        /// <summary>
        /// 事件发布时间
        /// </summary>
        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}