using Hao.AppService.ViewModel;
using Hao.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class ModuleAppService : ApplicationService, IModuleAppService
    {


        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public Task AddModule(ModuleAddRequest vm)
        {
            throw new NotImplementedException();
        }
    }
}
