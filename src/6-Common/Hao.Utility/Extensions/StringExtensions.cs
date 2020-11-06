using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Hao.Utility
{
    /// <summary>
    /// string扩展方法
    /// </summary>
    public static  class StringExtensions
    {
        /// <summary>
        /// 安全转换为字符串，去除两端空格，当值为null时返回""
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSafeString(this object value)
        {
            return value?.ToString().Trim() ?? string.Empty;
        }

        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 字符串是否有值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 转化成md5加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5(this string value)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(value);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 泛型集合转换
        /// </summary>
        /// <typeparam name="T">目标元素类型</typeparam>
        /// <param name="input">以逗号分隔的元素集合字符串，范例:83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A</param>
        public static List<T> ToList<T>(string value)
        {
            var result = new List<T>();
            if (string.IsNullOrWhiteSpace(value)) return result;
            var array = value.Split(',');
            result.AddRange(from each in array where !string.IsNullOrWhiteSpace(each) select To<T>(each));
            return result;
        }

        /// <summary>
        /// 通用泛型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="input">输入值</param>
        public static T To<T>(object value)
        {
            if (value == null)
                return default(T);
            if (value is string && string.IsNullOrWhiteSpace(value.ToString()))
                return default(T);
            Type type = H_Common.GetType<T>();
            var typeName = type.Name.ToLower();
            try
            {
                if (typeName == "string")
                    return (T)(object)value.ToString();
                if (typeName == "guid")
                    return (T)(object)new Guid(value.ToString());
                if (type.IsEnum)
                    return EnumExtensions.Parse<T>(value);
                if (value is IConvertible)
                    return (T)System.Convert.ChangeType(value, type);
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }
    }
}
