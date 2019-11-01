using System.Collections.Generic;

namespace Hao.Core.Model
{
    public class PagedList<T>
    {
        /// <summary>
        /// 列表
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// 列表总数
        /// </summary>
        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPagesCount { get; set; }

    }
}
