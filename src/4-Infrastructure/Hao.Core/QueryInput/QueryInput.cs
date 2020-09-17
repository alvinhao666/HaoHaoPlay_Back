using System.ComponentModel.DataAnnotations;

namespace Hao.Core
{
    /// <summary>
    /// 输入的查询条件
    /// </summary>
    public class QueryInput : IPagedQuery
    {
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex必须大于0")]
        public int PageIndex { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize不能超过100")] //防止恶意大量数据查询而导致数据库瘫痪
        public int PageSize { get; set; } = 10;

        public H_OrderByType? OrderByType { get; set; }
    }

    public enum H_OrderByType
    {
        Asc = 0,
        Desc = 1
    }
}
