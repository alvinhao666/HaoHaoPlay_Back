using System;
using System.ComponentModel;
using System.Reflection;

namespace Hao.Utility
{
    /// <summary>
    /// 枚举说明
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)] //枚举 或者 字段
    public class H_DescriptionAttribute: DescriptionAttribute
    {
        internal FieldInfo Field { get; set; }

        /// <summary>
        /// 枚举说明
        /// </summary>
        /// <param name="description">说明</param>

        public H_DescriptionAttribute(string description = null) : base(description)
        {

        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                FieldInfo field = this.Field;
                if (field == null)
                {
                    return null;
                }
                return field.Name;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public IConvertible Value
        {
            get
            {
                return (IConvertible)this.Field.GetValue(null);
            }
        }
    }
}
