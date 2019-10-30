using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class HUtil
    {
        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>  
        /// 获取指定时间戳  
        /// </summary>  
        /// <returns></returns> 
        public static string GetTimeStamp(DateTime time)
        {
            TimeSpan ts = time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
