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
    public static class H_EnumDescription
    {
        private static readonly ConcurrentDictionary<Type, List<H_EnumDescriptionAttribute>> _enumCache = new ConcurrentDictionary<Type, List<H_EnumDescriptionAttribute>>();
        
        internal static H_EnumDescriptionAttribute Get(Type enumType, string fieldName)
        {
            return Get(enumType).SingleOrDefault(d => d.Name == fieldName); // SingleOrDefault只取一个 如果没有数据等于 null， 如果>1异常
        }

        private static H_EnumDescriptionAttribute Get(FieldInfo fieldInfo)
        {
            var customAttribute = fieldInfo.GetCustomAttribute<H_EnumDescriptionAttribute>();
            if (customAttribute == null) return null;
            customAttribute.Field = fieldInfo;
            return customAttribute;
        }
        
        
        private static H_EnumDescriptionAttribute Get(Type enumType, int value)
        {
            return Get(enumType).SingleOrDefault(d => (int)d.Value == value);
        }

        
        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<H_EnumDescriptionAttribute> Get(Type enumType)
        {
            if (enumType.IsEnum)
            {
                return _enumCache.GetOrAdd(enumType, type => type.GetFields(BindingFlags.Static | BindingFlags.Public).Select(Get).ToList());
            }
            
            return new List<H_EnumDescriptionAttribute>();
        }


        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string GetDescription(Type enumType, int value)
        {
            var description = Get(enumType, value);

            return description?.Description;
        }


        /// <summary>
        /// 根据描述获取枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static TEnum? ToEnum<TEnum>(string description) where TEnum : struct, Enum
        {
            var descriptions = Get(typeof(TEnum));
            var type = descriptions.SingleOrDefault(a => a.Description == description);
            return (TEnum?) type?.Value;
        }
    }
}