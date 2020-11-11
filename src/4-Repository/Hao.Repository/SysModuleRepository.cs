using Hao.Core;
using Hao.Enum;
using Hao.Model;
using SqlSugar;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysModuleRepository : Repository<SysModule, long>, ISysModuleRepository
    {
        /// <summary>
        /// 获取每一层的数量，包括已删除的，最多31个 0~30
        /// </summary>
        /// <returns></returns>
        public async Task<ModuleLayerCountDto> GetLayerCount()
        {
            //var result = await Db.SqlQueryable<ModuleLayerCountDto>("select layer,count(*) from sysmodule group by layer order by layer desc limit 1").FirstAsync();
            //return result;

            //SELECT  "layer" AS "layer" , COUNT("layer") AS "count"  FROM "sysmodule"   GROUP BY "layer"  ORDER BY "layer" DESC LIMIT 1 offset 0;
            return await DbContext.Queryable<ModuleLayerCountDto>().AS("sysmodule")
                .GroupBy(a => new { a.Layer })
                .Select(a => new ModuleLayerCountDto { Layer = a.Layer, Count = SqlFunc.AggregateCount(a.Layer) })
                .OrderBy(a => a.Layer, OrderByType.Desc)
                .FirstAsync();
        }

        /// <summary>
        /// 是否存在相同名字的模块
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistSameNameModule(string name, ModuleType? moduleType, long? parentId, long? id = null)
        {
            var modules = await DbContext.Queryable<SysModule>()
                .Where(a => a.Name == name)
                .WhereIF(moduleType.HasValue, a => a.Type == moduleType)
                .WhereIF(parentId.HasValue, a => a.ParentId == parentId)
                .WhereIF(id.HasValue, a => a.Id != id)
                .ToListAsync();

            return modules.Count > 0;
        }
    }
}
