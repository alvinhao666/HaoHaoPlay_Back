using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hao.Utility
{
    public static class HConvert
    {
        public static int? ToInt(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !int.TryParse(value.ToString(), out var result)) 
                return null;
            else
                return result;
        }

        public static int ToInt0(this object value)
        {
            int result = 0;
            if (value != null && value != DBNull.Value)
                int.TryParse(value.ToString(), out result);
            return result;
        }

        public static float? ToFloat(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !float.TryParse(value.ToString(), out var result))
                return null;
            else
                return result;
        }

        public static float ToFloat0(this object value)
        {
            float result = 0f;
            if (value != null && value != DBNull.Value)
                float.TryParse(value.ToString(), out result);
            return result;
        }

        public static decimal? ToDecimal(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !decimal.TryParse(value.ToString(), out var result))
                return null;
            else
                return result;
        }

        public static decimal ToDecimal0(this object value)
        {
            decimal result = 0m;
            if (value != null && value != DBNull.Value)
                decimal.TryParse(value.ToString(), out result);
            return result;
        }

        public static double? ToDouble(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !double.TryParse(value.ToString(), out var result))
                return null;
            else
                return result;
        }

        public static double ToDouble0(this object value)
        {
            double result = 0d;
            if (value != null && value != DBNull.Value)
                double.TryParse(value.ToString(), out result);
            return result;
        }

        public static long? ToLong(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !long.TryParse(value.ToString(), out var result))
                return null;
            else
                return  result;
        }

        public static long ToLong0(this object value)
        {
            long result = 0L;
            if (value != null && value != DBNull.Value)
                long.TryParse(value.ToString(), out result);
            return result;
        }

        public static bool? ToBool(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !bool.TryParse(value.ToString(), out var result))
                return null;
            else
                return result;
        }

        public static bool ToBool0(this object value)
        {
            var a = value?.ToString().ToLower();
            var result = (a == "true" || a == "1");
            return result;
        }

        public static Guid? ToGuid(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !Guid.TryParse(value.ToString(), out var result))
                return null;
            else
                return result;
        }

        public static DateTime? ToDateTime(this object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) || !DateTime.TryParse(value.ToString(), out var result))
                return null;
            else
                return result;
        }
      
        /// <summary>
        /// 将金额转换成大写人民币
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string ToUpperRMB(this double money)
        {
            if (money < 0.01)
            {
                throw new ArgumentOutOfRangeException("money", "转换成大写人民币失败，金额最小单位为分");
            }
            return Regex.Replace(Regex.Replace(money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A"), "((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\\.]|$))))", "${b}${z}"), ".", (Match m) => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[(int)(m.Value[0] - '-')].ToString());
        }

        ///// <summary>
        ///// 将指定集合按照“分隔符”拼成字符串
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="lst">List集合</param>
        ///// <param name="separator">分隔符</param>
        ///// <returns>字符串</returns>
        //public static string ListToString<T>(IEnumerable<T> lst, char separator)
        //{
        //    if (lst == null || lst.Count<T>() == 0)
        //    {
        //        return string.Empty;
        //    }
        //    string result = string.Empty;
        //    StringBuilder stringBuilder = new StringBuilder();
        //    foreach (T item in lst)
        //    {
        //        stringBuilder.Append(separator.ToString() + item.ToString());
        //    }
        //    if (stringBuilder.ToString().Length > 0)
        //    {
        //        result = stringBuilder.ToString().Substring(1, stringBuilder.ToString().Length - 1);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 将字符串转变成List集合类型
        ///// </summary>
        ///// <param name="str">要转换的字符串</param>
        ///// <param name="split">分隔符</param>
        ///// <returns>List集合</returns>
        //public static List<string> ToList(string str, char split)
        //{
        //    return HConvert.ToList<string>(str, split);
        //}

        ///// <summary>
        ///// 将字符串转变成List集合类型
        ///// </summary>
        ///// <typeparam name="T">类型，目前只支持int,string</typeparam>
        ///// <param name="str">要转换的字符串</param>
        ///// <returns>List集合</returns>
        //public static List<T> ToList<T>(string str)
        //{
        //    return HConvert.ToList<T>(str, ',');
        //}

        ///// <summary>
        ///// 将字符串转变成List集合类型
        ///// </summary>
        ///// <typeparam name="T">类型，目前只支持int,string</typeparam>
        ///// <param name="str">要转换的字符串</param>
        ///// <param name="split">分隔符</param>
        ///// <returns>List集合</returns>
        //public static List<T> ToList<T>(string str, char split)
        //{
        //    if (!string.IsNullOrEmpty(str))
        //    {
        //        List<T> list = new List<T>();
        //        foreach (string text in str.Trim(new char[] { split }).Split(new char[] { split }))
        //        {
        //            if (!string.IsNullOrEmpty(text))
        //            {
        //                list.Add(UtilCommon.ConvertTo<T>(text, UtilCommon.DefaultOf<T>()));
        //            }
        //        }
        //        return list;
        //    }
        //    return new List<T>();
        //}
    }
}
