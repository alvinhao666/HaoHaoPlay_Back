using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hao.Utility
{
    public static class HValidator
    {

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email">Email地址</param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, "^[A-Za-z0-9](([_\\.\\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\\.\\-]?[a-zA-Z0-9]+)*)\\.([A-Za-z]{2,})$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证网址是否有效
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            return Regex.IsMatch(url, "^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\\.))+(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(/[a-zA-Z0-9\\&amp;%_\\./-~-]*)?$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证手机号，目前只支持中国手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            return Regex.IsMatch(mobile, "^1\\d{10}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证IP是否有效
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, "^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证身份证是否有效，目前只支持中国身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns></returns>
        public static bool IsIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                return HValidator.IsIDCard18(Id);
            }
            return Id.Length == 15 && HValidator.IsIDCard15(Id);
        }

        public static bool IsIDCard18(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out num))
            {
                return false;
            }
            if ("11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91".IndexOf(Id.Remove(2)) == -1)
            {
                return false;
            }
            string s = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime dateTime = default(DateTime);
            if (!DateTime.TryParse(s, out dateTime))
            {
                return false;
            }
            string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(new char[]
            {
                ','
            });
            string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(new char[]
            {
                ','
            });
            char[] array3 = Id.Remove(17).ToCharArray();
            int num2 = 0;
            for (int i = 0; i < 17; i++)
            {
                num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
            }
            int num3 = -1;
            Math.DivRem(num2, 11, out num3);
            return !(array[num3] != Id.Substring(17, 1).ToLower());
        }

        public static bool IsIDCard15(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id, out num) || (double)num < Math.Pow(10.0, 14.0))
            {
                return false;
            }
            if ("11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91".IndexOf(Id.Remove(2)) == -1)
            {
                return false;
            }
            string s = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime dateTime = default(DateTime);
            return DateTime.TryParse(s, out dateTime);
        }

        /// <summary>
        /// 是不是Int型的
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {
            return new Regex("^(-){0,1}\\d+$").Match(str).Success && long.Parse(str) <= 2147483647L && long.Parse(str) >= -2147483648L;
        }

        /// <summary>
        /// 看字符串的长度是不是在限定数之间（一个中文为两个字符）
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="begin">大于等于</param>
        /// <param name="end">小于等于</param>
        /// <returns></returns>
        public static bool IsLengthStr(string source, int begin, int end)
        {
            int length = Regex.Replace(source, "[^\\x00-\\xff]", "OK").Length;
            return length >= begin && length <= end;
        }

        /// <summary>
        /// 目前只支持中国电话，格式010-85849685
        /// </summary>
        /// <param name="str">电话号码</param>
        /// <returns></returns>
        public static bool IsTel(string source)
        {
            return Regex.IsMatch(source, "^\\d{3,4}-?\\d{6,8}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 目前只支持中国邮政编码，即6个数字
        /// </summary>
        /// <param name="str">邮编号码</param>
        /// <returns></returns>
        public static bool IsPostCode(string str)
        {
            return Regex.IsMatch(str, "^\\d{6}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证字符是否为中文汉字
        /// </summary>
        /// <param name="str">汉字字符串</param>
        /// <returns></returns>
        public static bool IsChineseChar(string str)
        {
            return Regex.IsMatch(str, "^[\\u4e00-\\u9fa5]+$", RegexOptions.IgnoreCase);
        }


        public static bool hasChineseChar(string str)
        {
            return Regex.IsMatch(str, "[\\u4e00-\\u9fa5]+", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证是不是正常字符 字母，数字，下划线的组合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNormalChar(string str)
        {
            return Regex.IsMatch(str, "^[a-zA-Z][a-zA-Z0-9_]*$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证给定的URL是否为图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsImage(string url)
        {
            return !string.IsNullOrWhiteSpace(url) && new string[]
            {
                ".jpeg",
                ".jpg",
                ".png",
                ".tif",
                ".tiff",
                ".bmp",
                ".gif"
            }.FirstOrDefault((string d) => url.ToLower().EndsWith(d)) != null;
        }

        /// <summary>
        /// 是否是默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefault<T>(this T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

        public static bool IsInRange(this int thisValue, int begin, int end)
        {
            return thisValue >= begin && thisValue <= end;
        }

        public static bool IsInRange(this DateTime thisValue, DateTime begin, DateTime end)
        {
            return thisValue >= begin && thisValue <= end;
        }

        public static bool IsIn<T>(this T thisValue, params T[] values)
        {
            return values.Contains(thisValue);
        }

        public static bool IsContainsIn(this string thisValue, params string[] inValues)
        {
            return inValues.Any(it => thisValue.Contains(it));
        }

        public static bool IsValuable(this IEnumerable<KeyValuePair<string, string>> thisValue)
        {
            if (thisValue == null || thisValue.Count() == 0) return false;
            return true;
        }

        public static bool IsZero(this object thisValue)
        {
            return (thisValue == null || thisValue.ToString() == "0");
        }

        public static bool IsInt(this object thisValue)
        {
            if (thisValue == null) return false;
            return Regex.IsMatch(thisValue.ToString(), @"^\d+$");
        }

        public static bool IsNoInt(this object thisValue)
        {
            if (thisValue == null) return true;
            return !Regex.IsMatch(thisValue.ToString(), @"^\d+$");
        }

        public static bool IsMoney(this object thisValue)
        {
            if (thisValue == null) return false;
            double outValue = 0;
            return double.TryParse(thisValue.ToString(), out outValue);
        }
        public static bool IsGuid(this object thisValue)
        {
            if (thisValue == null) return false;
            Guid outValue = Guid.Empty;
            return Guid.TryParse(thisValue.ToString(), out outValue);
        }

        public static bool IsDate(this object thisValue)
        {
            if (thisValue == null) return false;
            DateTime outValue = DateTime.MinValue;
            return DateTime.TryParse(thisValue.ToString(), out outValue);
        }

        public static bool IsEamil(this object thisValue)
        {
            if (thisValue == null) return false;
            return Regex.IsMatch(thisValue.ToString(), @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
        }

        public static bool IsMobile(this object thisValue)
        {
            if (thisValue == null) return false;
            return Regex.IsMatch(thisValue.ToString(), @"^\d{11}$");
        }

        public static bool IsTelephone(this object thisValue)
        {
            if (thisValue == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(thisValue.ToString(), @"^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{8}$");

        }

        public static bool IsIDcard(this object thisValue)
        {
            if (thisValue == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(thisValue.ToString(), @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
        }

        public static bool IsFax(this object thisValue)
        {
            if (thisValue == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(thisValue.ToString(), @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
        }

        public static bool IsMatch(this object thisValue, string pattern)
        {
            if (thisValue == null) return false;
            Regex reg = new Regex(pattern);
            return reg.IsMatch(thisValue.ToString());
        }
        public static bool IsAnonymousType(this Type type)
        {
            string typeName = type.Name;
            return typeName.Contains("<>") && typeName.Contains("__") && typeName.Contains("AnonymousType");
        }
        public static bool IsCollectionsList(this string thisValue)
        {
            return (thisValue + "").StartsWith("System.Collections.Generic.List") || (thisValue + "").StartsWith("System.Collections.Generic.IEnumerable");
        }
        public static bool IsStringArray(this string thisValue)
        {
            return (thisValue + "").IsMatch(@"System\.[a-z,A-Z,0-9]+?\[\]");
        }
        public static bool IsEnumerable(this string thisValue)
        {
            return (thisValue + "").StartsWith("System.Linq.Enumerable");
        }

        public static bool IsClass(this Type thisValue)
        {
            return thisValue != HUtil.StringType && thisValue.IsEntity();
        }
    }
}
