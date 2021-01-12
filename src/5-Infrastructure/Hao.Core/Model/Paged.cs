using System.Collections.Generic;

namespace Hao.Core
{
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
}
