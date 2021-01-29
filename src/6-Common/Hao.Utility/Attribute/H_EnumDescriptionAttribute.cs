using System;
using System.ComponentModel;
using System.Reflection;

namespace Hao.Utility
{
    /// <summary>
    /// 枚举说明
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)] //枚举 或者 字段
    public class H_EnumDescriptionAttribute: DescriptionAttribute
    {
        internal FieldInfo Field { get; set; }

        /// <summary>
        /// 枚举说明
        /// </summary>
        /// <param name="description">说明</param>

        public H_EnumDescriptionAttribute(string description = null) : base(description)
        {

        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name => Field?.Name;

        /// <summary>
        /// 值
        /// </summary>
        public IConvertible Value => (IConvertible) Field.GetValue(null);
    }
}
