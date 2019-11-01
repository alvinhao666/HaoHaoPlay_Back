using SqlSugar;
using System;

namespace Hao.Core
{
    public static class OrderByExtensions
    {
        /// <summary>
        /// 组合排序
        /// </summary>
        /// <param name="orderByType"></param>
        /// <param name="sortField"></param>
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
