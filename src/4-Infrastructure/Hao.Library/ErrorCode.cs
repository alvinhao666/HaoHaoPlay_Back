using System;

namespace Hao.Library
{
    public static class ErrorCode
    {

        public const string E100001 = "未授权或授权已过期，请重新登录";
        public const string E100002 = "用户未登录，请重新登录";
        public const string E100010 = "模型验证失败";
        //基础异常
        public const string E005001 = "用户不存在";
        public const string E005002 = "用户名已存在";
        public const string E005003 = "附件不存在";
        public const string E005004 = "两次输入的密码不相同";
        public const string E005005 = "用户名或密码错误";
        public const string E005006 = "用户未登录";
    }

    public static class StringExtensions
    {
        /// <summary>
        /// 返回错误Code
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetCode(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return int.Parse(str.Substring(1));
            }
            return 10000;
        }
    }
}
