using Hao.Core;
using Hao.Model;
using Hao.Utility;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysDictRepository: Repository<SysDict, long>, ISysDictRepository
    {

        public async Task<Paged<DictDto>> GetDictPagedResult(DictQuery query)
        {
            var items = await DbContext.Select<SysDict>()
                        .WhereIf(query.DictType.HasValue, a => a.DictType == query.DictType)
                        .WhereIf(query.DictName.HasValue(), a => a.DictName.Contains(query.DictName))
                        .WhereIf(query.DictCode.HasValue(), a => a.DictCode.Contains(query.DictCode))
                        .OrderByDescending(a => a.CreateTime)
                        .Count(out var total)
                        .Page(query.PageIndex, query.PageSize)
                        .ToListAsync(a => new DictDto
                        {
                            Id = a.Id,
                            ParentId = a.ParentId,
                            DictCode = a.DictCode,
                            DictName = a.DictName,
                            Remark = a.Remark,
                            CreateTime = a.CreateTime,
                            ItemNames = string.Join('，', DbContext.Select<SysDict>().Where(b => b.ParentId == a.Id).ToList(b => b.ItemName))
                        });


            return ToPaged(items, query.PageIndex, query.PageSize, total);
        }
    }
}
