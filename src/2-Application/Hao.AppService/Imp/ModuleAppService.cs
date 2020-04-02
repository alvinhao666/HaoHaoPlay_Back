using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hao.Enum;

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
            var parentNode = await GetModuleDetail(request.ParentId.Value);
            if (parentNode.Type == ModuleType.Sub) throw new HException("叶子节点无法继续添加节点");
            var modules = await _moduleRep.GetListAysnc(new ModuleQuery()
            {
                Name = request.Name,
                ParentId = request.ParentId
            });
            if (modules.Count > 0)
                throw new HException("模块名称已存在，请重新输入");
            var module = _mapper.Map<SysModule>(request);
            await _moduleRep.InsertAysnc(module);
        }

        /// <summary>
        /// 获取所有模块列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModuleVM>> GetList()
        {
            var modules = await _moduleRep.GetListAysnc();
            var result = new List<ModuleVM>();
            InitModuleTree(result, null, modules);
            return result;
        }

        /// <summary>
        /// 获取模块详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ModuleDetailVM> Get(long id)
        {
            var module = await GetModuleDetail(id);
            return _mapper.Map<ModuleDetailVM>(module);
        }

        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateModule(long id, ModuleUpdateRequest vm)
        {
            if (id == 0) throw new HException("无法操作系统根节点");
            var module = await GetModuleDetail(id);
            module.Name = vm.Name;
            module.Sort = vm.Sort;
            if (vm.Type == ModuleType.Main)
            {
                module.Icon = vm.Icon;
                await _moduleRep.UpdateAsync(module, user => new {module.Name, module.Icon, module.Sort});
            }
            else if (vm.Type == ModuleType.Sub)
            {
                module.RouterUrl = vm.RouterUrl;
                await _moduleRep.UpdateAsync(module, user => new {module.Name, module.RouterUrl, module.Sort});
            }
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            if (id == 0) throw new HException("无法操作系统根节点");
            var module = await GetModuleDetail(id);
            if (module.Type == ModuleType.Main)
            {
                var childs = await _moduleRep.GetListAysnc(new ModuleQuery()
                {
                    ParentId = module.ParentId
                });
                if (childs != null && childs.Count > 0) throw new HException("存在子节点无法删除");
            }

            await _moduleRep.DeleteAysnc(id);
        }


        #region private

        /// <summary>
        /// 递归初始化模块树
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parentID"></param>
        /// <param name="sources"></param>
        private void InitModuleTree(List<ModuleVM> result, long? parentID, List<SysModule> sources)
        {
            //递归寻找子节点  
            var tempTree = sources.Where(item => item.ParentId == parentID).OrderBy(a => a.Sort).ToList();
            foreach (var item in tempTree)
            {
                var node = new ModuleVM()
                {
                    key = item.Id.ToString(),
                    title = item.Name,
                    // Icon = item.Icon,
                    // RouterUrl = item.RouterUrl,
                    // ParentId = item.ParentId.ToString(),
                    isLeaf = item.Type == ModuleType.Sub,
                    type = item.Type,
                    children = new List<ModuleVM>()
                };
                result.Add(node);
                InitModuleTree(node.children, item.Id, sources);
            }
        }

        /// <summary>
        /// 获取模块详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<SysModule> GetModuleDetail(long id)
        {
            var module = await _moduleRep.GetAysnc(id);
            if (module == null) throw new HException("模块不存在");
            if (module.IsDeleted) throw new HException("模块已删除");
            return module;
        }

        #endregion
    }
}