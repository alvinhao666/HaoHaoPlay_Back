using Hao.Core;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysModuleRepository : Repository<SysModule, long>, ISysModuleRepository
    {
        public async Task<ModuleLayerCountInfo> GetLayerCount()
        {
            var result = await UnitOfWork.GetDbClient().SqlQueryable<ModuleLayerCountInfo>("select layer,count(*) from sysmodule group by layer order by layer desc limit 1").FirstAsync();
            return result;
        }
    }
}
