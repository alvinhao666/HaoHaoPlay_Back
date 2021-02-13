using System.Collections.Generic;

namespace Hao.Library
{
    public static class H_Error
    {
        ///// <summary>
        ///// 认证失败，请重新登录
        ///// </summary>
        //public static Lazy<KeyValuePair<int, string>> E100001 = new Lazy<KeyValuePair<int, string>>(() => new KeyValuePair<int, string>(100001, "认证失败，请重新登录"));

        /// <summary>
        /// 认证失败，请重新登录
        /// </summary>
        public static KeyValuePair<int, string> E100001 = new KeyValuePair<int, string>(100001, "认证失败，请重新登录");

        /// <summary>
        /// 用户未登录，请重新登录
        /// </summary>
        public static KeyValuePair<int, string> E100002 = new KeyValuePair<int, string>(100002, "用户未登录，请重新登录");

        /// <summary>
        /// 用户权限值已变更，请重新登录
        /// </summary>
        public static KeyValuePair<int, string> E100003 = new KeyValuePair<int, string>(100003, "用户权限值已变更，请重新登录");

        /// <summary>
        /// IP地址发生变更，请重新登录
        /// </summary>
        public static KeyValuePair<int, string> E100004 = new KeyValuePair<int, string>(100004, "IP地址发生变更，请重新登录");
    }
}
