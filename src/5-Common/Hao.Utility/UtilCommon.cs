using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    /// <summary>
    /// 基于工具类的工具类
    /// </summary>
    public static class UtilCommon
    {
        /// <summary>
        /// 将对象转换为目标类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        internal static T ConvertTo<T>(object src, T def)
        {
            Type type = typeof(T);
            if (type.Name == "Nullable`1" && type.IsValueType)
            {
                if (src == null || src.ToString() == string.Empty)
                {
                    return default(T);
                }
                type = type.GetGenericArguments()[0];
            }
            T result;
            try
            {
                if (type == typeof(Guid))
                {
                    result = (T)((object)new Guid(src.ToString()));
                }
                else if (type == typeof(Version))
                {
                    result = (T)((object)new Version(src.ToString()));
                }
                else
                {
                    result = (T)((object)Convert.ChangeType(src, type));
                }
            }
            catch
            {
                result = def;
            }
            return result;
        }

       

        /// <summary>
        /// 获取指定类型的非空默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T DefaultOf<T>()
        {
            Type typeFromHandle = typeof(T);
            if (typeFromHandle.FullName == "System.String")
            {
                return (T)((object)string.Empty);
            }
            if (typeFromHandle.IsValueType)
            {
                return default(T);
            }
            return Activator.CreateInstance<T>();
        }
    }
}
