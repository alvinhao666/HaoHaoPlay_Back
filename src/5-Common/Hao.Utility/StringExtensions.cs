using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Hao.Utility
{
    /// <summary>
    /// string扩展方法
    /// </summary>
    public static  class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        #region 转换成base64
        public static string ToBase64String(this string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        }

        public static string ToBase64String(this byte[] inArray)
        {
            return Convert.ToBase64String(inArray);
        }

        public static string ToBase64String(this string s, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(s));
        }

        public static string ToBase64String(this byte[] inArray, Base64FormattingOptions options)
        {
            return Convert.ToBase64String(inArray, options);
        }
        #endregion

        /// <summary>
        /// 将文本字符串转换为 URL 编码的字符串。
        /// </summary>
        /// <param name="url">要进行 URL 编码的文本。</param>
        /// <returns>URL 编码的字符串。</returns>
        public static string UrlEncode (this string url)
        {
            return WebUtility.UrlEncode(url);
        }
        /// <summary>
        /// 将已编码用于 URL 传输的字符串转换为解码的字符串。
        /// </summary>
        /// <param name="url">要解码的 URL 编码的字符串。</param>
        /// <returns>返回 System.String。已解码的字符串。</returns>
        public static string UrlDecode(this string url)
        {
            return WebUtility.UrlDecode(url);
        }

        public static string Join(this IEnumerable<string> values)
        {
            return string.Join(",", values);
        }

        public static string Join(this object[] values)
        {
            return string.Join(",", values);
        }

        public static string Join(this string[] value)
        {
            return string.Join(",", value);
        }

        public static string Join<T>(this IEnumerable<T> values)
        {
            return string.Join<T>(",", values);
        }

        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string Join(this object[] values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string Join(this string[] value, string separator)
        {
            return string.Join(separator, value);
        }

        public static string Join<T>(this IEnumerable<T> values, string separator)
        {
            return string.Join<T>(separator, values);
        }

        public static string Join(this string[] value, int startIndex, int count)
        {
            return string.Join(",", value, startIndex, count);
        }

        public static string Join(this string[] value, string separator, int startIndex, int count)
        {
            return string.Join(separator, value, startIndex, count);
        }
    }
}
