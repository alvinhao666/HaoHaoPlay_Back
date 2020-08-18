using Hao.AppService.ViewModel;
using Hao.Enum;
using Hao.Model;
using Hao.RunTimeException;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 模块应用服务-资源
    /// </summary>
    public partial class ModuleAppService
    {
        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [DistributedLock("ModuleAppService_AddResource")]
        public async Task AddResource(ResourceAddRequest vm)
        {
            var parentNode = await GetModuleDetail(vm.ParentId.Value);

            if (parentNode.Type != ModuleType.Sub) throw new H_Exception("非子菜单无法添加资源");

            var module = _mapper.Map<SysModule>(vm);
            module.Type = ModuleType.Resource;
            module.Sort = 0;
            await AddModule(module);
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteResource(long id)
        {
            if (id == 0) throw new H_Exception("无法操作系统根节点");

            var node = await GetModuleDetail(id);
            if (node.Type != ModuleType.Resource) throw new H_Exception("非资源项无法删除");

            await _moduleRep.DeleteAysnc(id);
        }

        /// <summary>
        /// 资源列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResourceItemVM>> GetResourceList(long parentId)
        {
            var resources = await _moduleRep.GetListAysnc(new ModuleQuery()
            {
                ParentId = parentId,
                OrderFileds = $"{nameof(SysModule.Sort)},{nameof(SysModule.CreateTime)}"
            });

            var result = _mapper.Map<List<ResourceItemVM>>(resources);
            return result;
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateResource(long id, ResourceUpdateRequest vm)
        {
            if (id == 0) throw new H_Exception("无法操作系统根节点");
            var module = await GetModuleDetail(id);
            module.Name = vm.Name;
            module.Sort = vm.Sort;
            if (module.Type != ModuleType.Resource) throw new H_Exception("非资源项无法更新");
            await _moduleRep.UpdateAsync(module, user => new { module.Name, module.Sort });
        }
    }
}
