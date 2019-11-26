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
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null) return false;
            Regex reg = new Regex(pattern);
            return reg.IsMatch(value.ToString());
        }

        public static bool IsInt(this string value)
        {
            return new Regex("^(-){0,1}\\d+$").Match(value).Success && long.Parse(value) <= 2147483647L && long.Parse(value) >= -2147483648L;
        }

        public static bool IsDate(this string value)
        {
            if (value == null) return false;
            return DateTime.TryParse(value.ToString(), out var outValue);
        }

        public static bool IsCollectionsList(this string value)
        {
            return (value + "").StartsWith("System.Collections.Generic.List") || (value + "").StartsWith("System.Collections.Generic.IEnumerable");
        }
        public static bool IsStringArray(this string value)
        {
            return (value + "").IsMatch(@"System\.[a-z,A-Z,0-9]+?\[\]");
        }
        public static bool IsEnumerable(this string value)
        {
            return (value + "").StartsWith("System.Linq.Enumerable");
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


        public static string ToPublicPem(this string publicKey)
        {
            return string.Format("-----BEGIN PUBLIC KEY-----\n{0}\n-----END PUBLIC KEY-----", publicKey);
        }

        public static string ToPrivatePem(this string privateKey)
        {
            return string.Format("-----BEGIN PRIVATE KEY-----\n{0}\n-----END PRIVATE KEY-----", privateKey);
        }
    }
}
