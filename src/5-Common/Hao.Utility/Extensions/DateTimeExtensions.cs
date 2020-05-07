using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 格式化时间，默认yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime? time, string format = null)
        {
            if (time.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(format))
                {
                    return time.Value.ToString(format);
                }
                return time.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return "";
        }

        /// <summary>
        /// 转换成开始时间字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToStartDateString(this DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("yyyy-MM-dd") + "00:00:00" : "";
        }

        /// <summary>
        /// 转换成结束时间字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>

        public static string ToEndDateString(this DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("yyyy-MM-dd") + "23:59:59" : "";
        }

        /// <summary>
        /// 转换成开始时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? ToStartDate(this DateTime? time)
        {
            if (!time.HasValue) return null;
            return DateTime.Parse(time.Value.ToString("yyyy-MM-dd") + "00:00:00");
        }

        /// <summary>
        /// 转换成结束时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? ToEndDate(this DateTime? time)
        {
            if (!time.HasValue) return null;
            return DateTime.Parse(time.Value.ToString("yyyy-MM-dd") + "23:59:59");
        }
    }
}
