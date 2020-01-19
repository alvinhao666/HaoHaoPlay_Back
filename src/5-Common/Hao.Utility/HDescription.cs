using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Hao.Utility
{
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    public class HDescription
    {

        private static readonly ConcurrentDictionary<Type, List<HDescriptionAttribute>> enumCache = new ConcurrentDictionary<Type, List<HDescriptionAttribute>>();

        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>

        public static List<HDescriptionAttribute> Get(Type enumType)
        {
            if (enumType.IsEnum)
            {
                return enumCache.GetOrAdd(enumType, type => type.GetFields(BindingFlags.Static | BindingFlags.Public).Select(a => Get(a)).ToList());
            }
            return new List<HDescriptionAttribute>();
        }


        private static HDescriptionAttribute Get(FieldInfo fieldInfo)
        {
            var customAttribute = fieldInfo.GetCustomAttribute<HDescriptionAttribute>();
            if (customAttribute == null) return null;
            customAttribute.Field = fieldInfo;
            return customAttribute;
        }


        internal static HDescriptionAttribute Get(Type enumType, string fieldName)
        {
            return Get(enumType).SingleOrDefault(d => d.Name == fieldName);
        }

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enum">枚举对象</param>
        ///// <returns></returns>
        //public static string GetDescription(Enum @enum)
        //{
        //    if (@enum == null) return null;
        //    var fieldName = @enum.ToString();
        //    var enumType = @enum.GetType();
        //    if (!Enum.IsDefined(enumType, @enum)) return null;
        //    var hDescriptionAttribute = Get(enumType, fieldName);
        //    return hDescriptionAttribute?.Description;
        //}

        ///// <summary>
        ///// 获取字段
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //public static HDescriptionAttribute Get<T>(Type enumType, T value) where T : struct, IConvertible
        //{
        //    return HDescription.Get(enumType).SingleOrDefault(delegate (HDescriptionAttribute item)
        //    {
        //        T t = (T)((object)item.Value);
        //        return t.Equals(value);
        //    });
        //}

        ///// <summary>
        ///// 获取值
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <returns></returns>
        //public static IConvertible GetValue(Type enumType, string fieldName)
        //{
        //    HDescriptionAttribute HDescriptionAttribute = HDescription.Get(enumType, fieldName);
        //    if (HDescriptionAttribute == null)
        //    {
        //        return null;
        //    }
        //    return HDescriptionAttribute.Value;
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
        //    HDescriptionAttribute HDescriptionAttribute = HDescription.Get(enumType, fieldName);
        //    if (HDescriptionAttribute == null)
        //    {
        //        return default(T);
        //    }
        //    return HDescriptionAttribute.GetValue<T>();
        //}

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <returns></returns>
        //public static string GetDescription(Type enumType, string fieldName)
        //{
        //    HDescriptionAttribute HDescriptionAttribute = HDescription.Get(enumType, fieldName);
        //    if (HDescriptionAttribute == null)
        //    {
        //        return null;
        //    }
        //    return HDescriptionAttribute.Description;
        //}

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //public static string GetDescription(Type enumType, int value)
        //{
        //    HDescriptionAttribute HDescriptionAttribute = HDescription.Get<int>(enumType, value);
        //    if (HDescriptionAttribute == null)
        //    {
        //        return null;
        //    }
        //    return HDescriptionAttribute.Description;
        //}

        ///// <summary>
        ///// 获取描述
        ///// </summary>
        ///// <param name="enumType">枚举类型</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //public static string GetDescription<T>(Type enumType, T value) where T : struct, IConvertible
        //{
        //    HDescriptionAttribute HDescriptionAttribute = HDescription.Get<T>(enumType, value);
        //    if (HDescriptionAttribute == null)
        //    {
        //        return null;
        //    }
        //    return HDescriptionAttribute.Description;
        //}
    }
}
