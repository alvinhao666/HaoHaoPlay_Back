using SqlSugar;

namespace Hao.Core
{
    public interface IQueryInput
    {
        OrderByType? OrderByType { get; set; }
    }
}
