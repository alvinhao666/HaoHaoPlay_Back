using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.Model
{
    public interface IsCreateAudited
    {
        /// <summary>
        /// 创建人
        /// </summary>
        long? CreaterId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateTime { get; set; }
    }
}
