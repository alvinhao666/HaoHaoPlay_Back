using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.Model
{
    public class PagedList<T>
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// 列表总数
        /// </summary>
        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPagesCount { get; set; }

    }
}
