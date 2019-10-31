using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public static class OrderByExtensions
    {
        /// <summary>
        /// 组合排序
        /// </summary>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public static string CombineNameWithSpace(this OrderByType? orderByType, Enum sortField)
        {
            if (orderByType.HasValue && sortField != null )
            {
                return string.Format("{0} {1}", sortField, orderByType);
            }
            return null;
        }
    }
}
