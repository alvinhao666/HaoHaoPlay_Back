using System;

namespace Hao.Core
{
    /// <summary>
    /// 修改信息接口
    /// </summary>
    public interface IsModifyAudited
    {
        /// <summary>
        /// 最后一次修改人id
        /// </summary>
        long? ModifierId { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        DateTime? ModifyTime { get; set; }
    }
}
