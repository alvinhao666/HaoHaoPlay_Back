namespace Hao.Core
{
    /// <summary>
    /// 分页查询接口
    /// </summary>
    public interface IPagedQuery
    {
        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        int PageSize { get; set; }
    }
}
