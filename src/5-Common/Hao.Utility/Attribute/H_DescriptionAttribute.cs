using System;
using System.ComponentModel;
using System.Reflection;

namespace Hao.Utility
{
    /// <summary>
    /// 枚举说明
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)] //枚举且是字段
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

        ///// <summary>
        ///// 值
        ///// </summary>
        //public T GetValue<T>() where T : struct, IConvertible
        //{
        //    return (T)((object)this.Value);
        //}

        ///// <summary>
        ///// 获取所有字段
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <returns></returns>
        //public static HDescriptionAttribute[] Get(Type enumType)
        //{
        //    return HDescription.Get(enumType);
        //}

        ///// <summary>
        ///// 获取字段
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <returns></returns>
        //public static HDescriptionAttribute Get(Type enumType, string fieldName)
        //{
        //    return HDescription.Get(enumType, fieldName);
        //}

        ///// <summary>
        ///// 获取字段
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //public static HDescriptionAttribute Get<T>(Type enumType, T value) where T : struct, IConvertible
        //{
        //    return HDescription.Get<T>(enumType, value);
        //}

        ///// <summary>
        ///// 获取值
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <returns></returns>
        //public static IConvertible GetValue(Type enumType, string fieldName)
        //{
        //    return HDescription.GetValue(enumType, fieldName);
        //}

        ///// <summary>
        ///// 获取值
        ///// </summary>
        ///// <typeparam name="T">值类型</typeparam>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <returns></returns>
        //public static T GetValue<T>(Type enumType, string fieldName) where T : struct, IConvertible
        //{
        //    return HDescription.GetValue<T>(enumType, fieldName);
        //}

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <returns></returns>
        //public static string GetDescription(Type enumType, string fieldName)
        //{
        //    return HDescription.GetDescription(enumType, fieldName);
        //}

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //public static string GetDescription(Type enumType, int value)
        //{
        //    return HDescription.GetDescription(enumType, value);
        //}

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //public static string GetDescription<T>(Type enumType, T value) where T : struct, IConvertible
        //{
        //    return HDescription.GetDescription<T>(enumType, value);
        //}

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enum">枚举对象</param>
        ///// <returns></returns>
        //public static string GetDescription(Enum @enum)
        //{
        //    return HDescription.GetDescription(@enum);
        //}
    }
}
