using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class BoolExtensions
    {
        /// <summary>
        /// 是否为真
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTrue(this bool? value)
        {
            return value.HasValue && value.Value;
        }

        /// <summary>
        /// 是否为假
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public static bool IsFalse(this bool? value)
        {
            return value.HasValue && !value.Value;
        }
    }
}
