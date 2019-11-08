namespace Hao.Query
{
    public interface IPagedQuery
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }
    }
}
