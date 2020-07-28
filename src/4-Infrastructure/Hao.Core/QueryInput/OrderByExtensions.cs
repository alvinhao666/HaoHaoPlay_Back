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
        public static string CombineNameWithSpace(this H_OrderByType? orderByType, Enum sortField)
        {
            if (orderByType.HasValue && sortField != null )
            { 
                return string.Format("{0} {1}", sortField, orderByType); //string.Format方法在内部使用StringBuilder进行字符串的格式化
            }
            return null;
        }
    }
}
