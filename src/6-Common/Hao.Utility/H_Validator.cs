using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hao.Utility
{
    public static class H_Validator
    {
        #region 实用功能判断

        /// <summary>
        /// 验证是否是字母
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLetter(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^[a-zA-Z]+$");
        }

        /// <summary>
        /// 验证是否是字母或者数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLetterOrDigit(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="value">Email地址</param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^[A-Za-z0-9](([_\\.\\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\\.\\-]?[a-zA-Z0-9]+)*)\\.([A-Za-z]{2,})$");
        }

        /// <summary>
        /// 验证手机号，目前只支持中国手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMobile(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^1\\d{10}$");
        }


        /// <summary>
        /// 目前只支持中国电话，格式010-85849685
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTel(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^\\d{3,4}-?\\d{6,8}$");
        }

        /// <summary>
        /// 验证IP是否有效
        /// </summary>
        /// <param name="value">IP地址</param>
        /// <returns></returns>
        public static bool IsIP(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 验证网址是否有效
        /// </summary>
        /// <param name="value">网址</param>
        /// <returns></returns>
        public static bool IsUrl(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\\.))+(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(/[a-zA-Z0-9\\&amp;%_\\./-~-]*)?$");
        }


        /// <summary>
        /// 目前只支持中国邮政编码，即6个数字
        /// </summary>
        /// <param name="value">邮编号码</param>
        /// <returns></returns>
        public static bool IsPostCode(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^\\d{6}$");
        }


        /// <summary>
        /// 验证身份证是否有效，目前只支持中国身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns></returns>
        public static bool IsIDCard(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            if (value.Length == 18)
            {
                return IsIDCard18(value);
            }
            return value.Length == 15 && IsIDCard15(value);
        }

        private static bool IsIDCard15(string value)
        {
            long num = 0L;
            if (!long.TryParse(value, out num) || (double)num < Math.Pow(10.0, 14.0))
            {
                return false;
            }
            if ("11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91".IndexOf(value.Remove(2)) == -1)
            {
                return false;
            }
            string s = value.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime dateTime = default(DateTime);
            return DateTime.TryParse(s, out dateTime);
        }

        private static bool IsIDCard18(string value)
        {
            long num = 0L;
            if (!long.TryParse(value.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(value.Replace('x', '0').Replace('X', '0'), out num))
            {
                return false;
            }
            if ("11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91".IndexOf(value.Remove(2)) == -1)
            {
                return false;
            }
            string s = value.Substring(6, 8).Insert(6, "-").Insert(4, "-");
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
            char[] array3 = value.Remove(17).ToCharArray();
            int num2 = 0;
            for (int i = 0; i < 17; i++)
            {
                num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
            }
            int num3 = -1;
            Math.DivRem(num2, 11, out num3);
            return !(array[num3] != value.Substring(17, 1).ToLower());
        }


        /// <summary>
        /// 验证字符是否为中文汉字
        /// </summary>
        /// <param name="value">汉字字符串</param>
        /// <returns></returns>
        public static bool IsChineseChar(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "^[\\u4e00-\\u9fa5]+$");
        }

        /// <summary>
        /// 是否含有中文汉字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool hasChineseChar(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, "[\\u4e00-\\u9fa5]+");
        }

        /// <summary>
        /// 验证给定的URL是否为图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsImage(string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            value = value.ToLower();
            return new string[]
            {
                ".jpeg",
                ".jpg",
                ".png",
                ".tif",
                ".tiff",
                ".bmp",
                ".gif"
            }.FirstOrDefault(d => value.EndsWith(d)) != null;
        }

        #endregion
    }
}
