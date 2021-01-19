using System;

namespace Hao.Core
{
    /// <summary>
    /// 创建信息接口
    /// </summary>
    public interface IsCreateAudited
    {
        /// <summary>
        /// 创建人id
        /// </summary>
        long? CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateTime { get; set; }
    }
}
