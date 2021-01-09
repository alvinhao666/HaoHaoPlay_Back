using Hao.Core;
using Hao.Enum;
using Hao.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysModuleRepository : Repository<SysModule, long>, ISysModuleRepository
    {
        /// <summary>
        /// 获取每一层的数量，包括已删除的，最多31个 0~30
        /// </summary>
        /// <returns></returns>
        public async Task<ModuleLayerCountDTO> GetLayerCount()
        {
            //var result = await Db.SqlQueryable<ModuleLayerCountDto>("select layer,count(*) from sysmodule group by layer order by layer desc limit 1").FirstAsync();
            //return result;

            //SELECT  "layer" AS "layer" , COUNT("layer") AS "count"  FROM "sysmodule"   GROUP BY "layer"  ORDER BY "layer" DESC LIMIT 1 offset 0;

            InitAsyncLocalCurrentUser();
            
            var list = await DbContext.Select<ModuleLayerCountDTO>()
                                        .DisableGlobalFilter(nameof(IsSoftDelete))
                                        .AsTable((_, oldname) => $"sysmodule")
                                        .GroupBy(a => new { a.Layer })
                                        .OrderByDescending(a => a.Key.Layer)
                                        .ToListAsync(a => new ModuleLayerCountDTO { Layer = a.Key.Layer, Count = a.Count() });

            return list.FirstOrDefault();
        }

        /// <summary>
        /// 是否存在相同名字的模块
        /// </summary>
        /// <param name="name"></param>
        /// <param name="moduleType"></param>
        /// <param name="parentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistSameNameModule(string name, ModuleType? moduleType, long? parentId, long? id = null)
        {
            InitAsyncLocalCurrentUser();
            
            var modules = await DbContext.Select<SysModule>()
                                            .Where(a => a.Name == name)
                                            .WhereIf(moduleType.HasValue, a => a.Type == moduleType)
                                            .WhereIf(parentId.HasValue, a => a.ParentId == parentId)
                                            .WhereIf(id.HasValue, a => a.Id != id)
                                            .ToListAsync();

            return modules.Count > 0;
        }

        /// <summary>
        /// 是否存在相同别名的模块
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="moduleType"></param>
        /// <param name="parentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistSameAliasModule(string alias, ModuleType? moduleType, long? parentId, long? id = null)
        {
            InitAsyncLocalCurrentUser();
            
            var modules = await DbContext.Select<SysModule>()
                                            .Where(a => a.Alias == alias)
                                            .WhereIf(moduleType.HasValue, a => a.Type == moduleType)
                                            .WhereIf(parentId.HasValue, a => a.ParentId == parentId)
                                            .WhereIf(id.HasValue, a => a.Id != id)
                                            .ToListAsync();

            return modules.Count > 0;
        }
    }
}
