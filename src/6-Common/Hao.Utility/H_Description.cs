using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hao.Utility
{
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    public class H_Description
    {
        private static readonly ConcurrentDictionary<Type, List<H_DescriptionAttribute>> _enumCache = new ConcurrentDictionary<Type, List<H_DescriptionAttribute>>();
        
        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<H_DescriptionAttribute> Get(Type enumType)
        {
            if (enumType.IsEnum)
            {
                return _enumCache.GetOrAdd(enumType, type => type.GetFields(BindingFlags.Static | BindingFlags.Public).Select(Get).ToList());
            }

            return new List<H_DescriptionAttribute>();
        }

        internal static H_DescriptionAttribute Get(Type enumType, string fieldName)
        {
            return Get(enumType).SingleOrDefault(d => d.Name == fieldName); // SingleOrDefault只取一个 如果没有数据等于 null， 如果>1异常
        }

        private static H_DescriptionAttribute Get(FieldInfo fieldInfo)
        {
            var customAttribute = fieldInfo.GetCustomAttribute<H_DescriptionAttribute>();
            if (customAttribute == null) return null;
            customAttribute.Field = fieldInfo;
            return customAttribute;
        }


        /// <summary>
        /// 根据描述获取枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(string description)
        {
            var descriptions = Get(typeof(TEnum));
            var type = descriptions.Single(a => a.Description == description);
            return (TEnum)type.Value;
        }
    }
}