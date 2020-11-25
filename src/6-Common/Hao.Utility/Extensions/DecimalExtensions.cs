using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// 金额保留两位小数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToMoneyString(this decimal? value)
        {
            if (!value.HasValue) return "";

            return value.Value.ToString("#0.##");
        }
    }
}
