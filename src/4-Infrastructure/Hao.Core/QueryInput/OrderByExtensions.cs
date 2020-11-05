using System;
using System.Text;

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
        public static string ToOrderByFields<T>(this T?[] sortFields, SortType?[] orderByTypes) where T : struct, Enum
        {
            if (orderByTypes == null || sortFields == null) return null;

            if (orderByTypes.Length == 0 || sortFields.Length == 0) return null;

            if (orderByTypes.Length != sortFields.Length) return null;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < sortFields.Length; i++)
            {
                if (sortFields[i].HasValue && orderByTypes[i].HasValue)
                {
                    sb.Append(string.Format("{0} {1},", sortFields[i], orderByTypes[i]));
                }
            }

            return sb.ToString().TrimEnd(',');
        }
    }
}
