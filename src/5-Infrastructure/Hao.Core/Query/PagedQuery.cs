namespace Hao.Core
{
    public abstract class PagedQuery : IPagedQuery
    {
        public virtual int PageIndex { get; set; } = 1;

        public virtual int PageSize { get; set; } = 10;
    }
}