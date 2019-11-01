using SqlSugar;

namespace Hao.Core.QueryInput
{
    public interface IQueryInput
    {
        OrderByType? OrderByType { get; set; }
    }
}
