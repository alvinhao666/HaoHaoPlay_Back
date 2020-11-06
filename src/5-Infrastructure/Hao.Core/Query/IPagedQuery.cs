namespace Hao.Core
{
    public interface IPagedQuery
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }
    }
}
