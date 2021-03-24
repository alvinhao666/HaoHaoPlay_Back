using Hao.Core;
using System.Threading.Tasks;

namespace Hao.Model
{
    public interface IDictRepository : IRepository<SysDict, long>
    {
        Task<Paged<DictDto>> GetDictPagedResult(DictQuery query);
    }
}
