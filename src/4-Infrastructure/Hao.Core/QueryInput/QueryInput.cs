namespace Hao.Core
{
    /// <summary>
    /// 输入的查询条件
    /// </summary>
    public class QueryInput : IPagedQuery
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public H_OrderByType? OrderByType { get; set; }
    }

    public enum H_OrderByType
    {
        Asc = 0,
        Desc = 1
    }
}
