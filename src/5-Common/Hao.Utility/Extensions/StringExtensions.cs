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
    }
}
