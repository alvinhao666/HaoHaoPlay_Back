using System;
using System.ComponentModel.DataAnnotations;

namespace Hao.Core
{
    /// <summary>
    /// 输入的查询条件
    /// </summary>
    public abstract class QueryInput : IPagedQuery
    {
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex必须大于0")]
        public virtual int PageIndex { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize不能超过100")] //防止恶意大量数据查询而导致数据库瘫痪
        public virtual int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序类型
        /// </summary>
        public SortType?[] SortTypes { get; set; }
    }

    public enum SortType
    {
        Asc = 0,
        Desc = 1
    }
}
