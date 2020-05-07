using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Hao.Utility
{
    /// <summary>
    /// string扩展方法
    /// </summary>
    public static  class StringExtensions
    {
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
        /// 字符串是否满足正则
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null) return false;
            Regex reg = new Regex(pattern);
            return reg.IsMatch(value.ToString());
        }

        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(this string value)
        {
            return new Regex("^(-){0,1}\\d+$").Match(value).Success && long.Parse(value) <= 2147483647L && long.Parse(value) >= -2147483648L;
        }

        /// <summary>
        /// 字符串是否为时间格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDate(this string value)
        {
            if (value == null) return false;
            return DateTime.TryParse(value.ToString(), out var outValue);
        }
    }
}
