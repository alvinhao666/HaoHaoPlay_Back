using System;

namespace Hao.Utility
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// 保留小数位数，默认2位，四舍五入  
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDecimalString(this decimal? obj, string format= "#0.##")
        {
            //0.995 ---- 1
            //0.994 ---- 0.99
            if (!obj.HasValue) return "0";

            return obj.Value.ToString(format);

            //string.Format("{0:N2}", x)   0.995 ---- 1.00
        }

        /// <summary>
        /// 保留小数位数，默认2位，四舍五入  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal KeepSmallDigit(this decimal? obj, int decimals = 2)
        {
            //0.995 ---- 1
            //0.994 ---- 0.99
            if (!obj.HasValue) return 0;
            return decimal.Round(obj.Value, decimals, MidpointRounding.AwayFromZero);
        }

    }
}
