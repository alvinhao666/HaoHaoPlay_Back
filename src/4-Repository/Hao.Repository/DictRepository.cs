using Hao.Core;
using Hao.Model;
using Hao.Utility;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class DictRepository : Repository<SysDict, long>, IDictRepository
    {

        public async Task<Paged<DictDto>> GetDictPagedResult(DictQuery query)
        {
            var items = await DbContext.Select<SysDict>()
                        .WhereIf(query.DictType.HasValue, a => a.DictType == query.DictType)
                        .WhereIf(query.LikeDictName.HasValue(), a => a.DictName.Contains(query.LikeDictName))
                        .WhereIf(query.LikeDictCode.HasValue(), a => a.DictCode.Contains(query.LikeDictCode))
                        .OrderByDescending(a => a.CreateTime)
                        .Count(out var totalCount)
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


            return items.ToPaged(query, totalCount);
        }
    }
}
