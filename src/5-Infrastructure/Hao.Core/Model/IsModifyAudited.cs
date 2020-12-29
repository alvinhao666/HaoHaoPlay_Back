using System;

namespace Hao.Core
{
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
