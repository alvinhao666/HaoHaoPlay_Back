using System.Collections.Generic;

namespace Hao.Core
{
    /// <summary>
    /// 分页类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paged<T>
    {
        /// <summary>
        /// 列表项
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 规定每一页条数大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总共页数
        /// </summary>
        public int TotalPageCount { get; set; }
        
    }

    /// <summary>
    /// 分页
    /// </summary>
    public static class Paged
    {
        /// <summary>
        /// 转换为分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="query"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static Paged<T> ToPaged<T>(this IEnumerable<T> items, IPagedQuery query, long totalCount) where T : class, new()
        {
            var pageList = new Paged<T>()
            {
                Items = items,
                TotalCount = (int)totalCount,
                PageIndex = query.PageIndex,
                PageSize =  query.PageSize,
                TotalPageCount = ((int)totalCount + query.PageSize - 1) / query.PageSize
            };
            return pageList;
        }
    }
}
