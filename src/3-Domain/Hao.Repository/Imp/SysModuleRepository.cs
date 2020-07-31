using Hao.Core;
using Hao.Model;
using SqlSugar;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysModuleRepository : Repository<SysModule, long>, ISysModuleRepository
    {
        /// <summary>
        /// 获取每一层的数量，包括已删除的，最多64个
        /// </summary>
        /// <returns></returns>
        public async Task<ModuleLayerCountDto> GetLayerCount()
        {
            //var result = await Db.SqlQueryable<ModuleLayerCountDto>("select layer,count(*) from sysmodule group by layer order by layer desc limit 1").FirstAsync();
            //return result;

            return await Db.Queryable<ModuleLayerCountDto>()
                .GroupBy(a => new { a.Layer })
                .Select(a => new ModuleLayerCountDto { Layer = a.Layer, Count = SqlFunc.AggregateCount(a.Layer) })
                .OrderBy(a => a.Layer, OrderByType.Desc)
                .FirstAsync();
        }
    }
}
