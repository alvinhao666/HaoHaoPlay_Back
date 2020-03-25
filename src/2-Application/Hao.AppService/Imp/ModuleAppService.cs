using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class ModuleAppService : ApplicationService, IModuleAppService
    {

        private readonly IMapper _mapper;

        private readonly ISysModuleRepository _moduleRep;

        public ModuleAppService(IMapper mapper, ISysModuleRepository moduleRep)
        {
            _mapper = mapper;
            _moduleRep = moduleRep;
        }


        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddModule(ModuleAddRequest request)
        {
            var modules = await _moduleRep.GetListAysnc(new ModuleQuery()
            {
                Name = request.Name
            });
            if (modules.Count > 0)
                throw new HException("模块名称已存在，请重新输入");
            var module = _mapper.Map<SysModule>(request);
            await _moduleRep.InsertAysnc(module);
        }
    }
}
