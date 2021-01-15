namespace Hao.Library
{
    public static class H_Error
    {
        public const string E100001 = "认证失败，请重新登录";
        public const string E100002 = "用户未登录，请重新登录";
        public const string E100003 = "用户权限值已变更，请重新登录";
        public const string E100004 = "IP地址发生变更，请重新登录";

        /// <summary>
        /// 返回错误Code
        /// </summary>
        /// <param name="errorName"></param>
        /// <returns></returns>
        public static int ToCode(this string errorName)
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
