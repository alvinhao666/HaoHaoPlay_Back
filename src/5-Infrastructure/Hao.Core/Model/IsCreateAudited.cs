using System;

namespace Hao.Core
{
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
