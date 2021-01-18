using System;
using System.ComponentModel.DataAnnotations;
using Hao.Utility;

namespace Hao.Core
{
    /// <summary>
    /// 输入的查询条件
    /// </summary>
    public abstract class QueryInput : IPagedQuery
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex范围1~2147483647")]
        public virtual int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        [Range(1, 300, ErrorMessage = "PageSize范围1~300")] //防止恶意大量数据查询而导致数据库瘫痪
        public virtual int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序类型
        /// </summary>
        public SortType?[] SortTypes { get; set; }
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        [H_Description("升序")]
        Asc = 0,

        [H_Description("降序")]
        Desc = 1
    }
}
