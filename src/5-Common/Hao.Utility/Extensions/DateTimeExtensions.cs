using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class DateTimeExtensions
    {
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

        public static string ToStartDateString(this DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("yyyy-MM-dd") + "00:00:00" : "";
        }

        public static string ToEndDateString(this DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("yyyy-MM-dd") + "23:59:59" : "";
        }

        public static DateTime? ToStartDate(this DateTime? time)
        {
            if (!time.HasValue) return null;
            return DateTime.Parse(time.Value.ToString("yyyy-MM-dd") + "00:00:00");
        }

        public static DateTime? ToEndDate(this DateTime? time)
        {
            if (!time.HasValue) return null;
            return DateTime.Parse(time.Value.ToString("yyyy-MM-dd") + "23:59:59");
        }
    }
}
