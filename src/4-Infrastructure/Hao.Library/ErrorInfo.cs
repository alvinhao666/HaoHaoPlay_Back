using System;

namespace Hao.Library
{
    public static class ErrorInfo
    {
        public const string E100001 = "认证失败，请重新登录";
        public const string E100002 = "未授权，请重新登录";
        public const string E100003 = "用户未登录，请重新登录";

    }

    public static class StringExtensions
    {
        /// <summary>
        /// 返回错误Code
        /// </summary>
        /// <param name="errorName"></param>
        /// <returns></returns>
        public static int GetErrorCode(this string errorName)
        {
            int result = 0;
            if (!string.IsNullOrWhiteSpace(errorName))
            {
                int.TryParse(errorName.Substring(1), out result);
            }
            return result;
        }
    }
}
