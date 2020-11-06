using System;
using System.Collections.Generic;

namespace Hao.Utility
{
    /// <summary>
    /// 泛型扩展类
    /// </summary>
    public static class GenericExtensions
    {
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

        /// <summary>
        /// 返回一个新对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>

        public static T IsNullReturnNew<T>(this T value) where T : new()
        {
            if (value.IsNullOrWhiteSpace())
            {
                value = new T();
            }
            return value;
        }

        private static bool IsNullOrWhiteSpace(this object value)
        {
            if (value == null || value == DBNull.Value || value.ToString().Trim() == "") return true;
            return false;
        }
    }
}
