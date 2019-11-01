using System;

namespace Hao.Core.Model
{
    public interface IsModifyAudited
    {
        /// <summary>
        /// 最后一次修改人
        /// </summary>
        long? LastModifyUserId { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        DateTime? LastModifyTime { get; set; }
    }
}
