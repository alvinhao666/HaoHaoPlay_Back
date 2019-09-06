using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.Model
{
    /// <summary>
    /// 表示继承于该接口的类型是领域实体类。
    /// </summary>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// 获取当前领域实体类的全局唯一标识。
        /// </summary>
        TKey ID { get; set; }
    }
}
