using Hao.Core;
using Hao.Model;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public interface ISysModuleRepository : IRepository<SysModule, long>
    {
        Task<ModuleLayerCountInfo> GetLayerCount();
    }
}
