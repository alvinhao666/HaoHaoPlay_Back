namespace Hao.Core
{
    /// <summary>
    /// 分页查询抽象类
    /// </summary>
    public abstract class PagedQuery : IPagedQuery
    {
        /// <summary>
        /// 页码，默认第1页
        /// </summary>
        public virtual int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量，默认每页10条数据
        /// </summary>
        public virtual int PageSize { get; set; } = 10;
    }
}