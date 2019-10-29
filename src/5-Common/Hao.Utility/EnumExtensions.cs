using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 组合排序
        /// </summary>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public static string CombineNameWithSpace(this Enum sortField, Enum orderType) 
        {
            if (orderType != null && sortField != null )
            {
                return string.Format("{0} {1}", sortField, orderType);
            }
            return null;
        }
    }
}
