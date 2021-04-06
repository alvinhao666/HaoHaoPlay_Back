using System;
using System.Collections.Generic;

namespace Hao.Core
{
    /// <summary>
    /// 异常类
    /// </summary>
    public class H_Exception : Exception
    {
        /// <summary>
        /// 异常代码
        /// </summary>
        public int? Code { get; private set; }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public H_Exception(string message) : base(message) { }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="pair">key:异常代码值，value:异常消息</param>
        public H_Exception(KeyValuePair<int, string> pair) : base(pair.Value)
        {
            Code = pair.Key;
        }
    }

    /// <summary>
    /// 断言异常
    /// </summary>
    public static class H_AssertEx
    {
        /// <summary>
        /// 断言异常
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        /// <exception cref="H_Exception"></exception>
        public static void That(bool condition, string msg)
        {
            if (condition)
            {
                throw new H_Exception(msg);
            }
        }
    }
}
