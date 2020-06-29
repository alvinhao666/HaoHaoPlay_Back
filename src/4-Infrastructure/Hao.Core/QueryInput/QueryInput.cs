using Hao.Query;
using SqlSugar;

namespace Hao.Core
{
    public class QueryInput : IPagedQuery, IQueryInput
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public OrderByType? OrderByType { get; set; }
    }
}
