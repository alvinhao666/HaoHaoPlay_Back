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

        /// <summary>
        /// 获取Guid
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 对指定对象执行委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        public static T With<T>(this T item, Action<T> work)
        {
            if (item == null)
            {
                return default(T);
            }
            work(item);
            return item;
        }


        /// <summary>
        /// 获得中文字符串的首字母
        /// </summary>
        /// <param name="str">中文字符串</param>
        /// <returns></returns>
        public static string GetInitialSpells(string str)
        {
            return HSpell.GetSpell(str);
        }


        /// <summary>
        /// 用来获得一个字的拼音首字母
        /// </summary>
        /// <param name="cnChar">一个字</param>
        /// <returns></returns>
        public static string GetInitialSpell(string cnChar)
        {
            if (string.IsNullOrEmpty(cnChar))
            {
                return cnChar;
            }
            return HSpell.GetSpell(cnChar[0]);
        }

        /// <summary>
        /// 获得姓名的缩写，例如zhangs
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        public static string GetNameSpells(string cnChar)
        {
            if (string.IsNullOrEmpty(cnChar))
            {
                return cnChar;
            }
            string text = "";
            for (int i = 0; i < cnChar.Length; i++)
            {
                text += ((i == 0) ? HSpell.GetSpells(cnChar[i]).ToLower() : HSpell.GetSpell(cnChar[i]).ToLower());
            }
            return text;
        }

    }
}
