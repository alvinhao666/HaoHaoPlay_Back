using System;
using System.Collections.Generic;
using System.Text;
using Hao.Utility;

namespace Hao.Enum
{
    /// <summary>
    /// 字典类型
    /// </summary>
    [H_EnumDescription("字典类型")]
    public enum DictType
    {
        /// <summary>
        /// 字典
        /// </summary>
        [H_EnumDescription("字典")]
        Main,
        
        /// <summary>
        /// 字典项
        /// </summary>
        [H_EnumDescription("字典项")]
        Sub
    }
}
