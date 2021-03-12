using Hao.Core;
using Hao.Enum;
using Hao.Model;
using Mapster;
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
        /// <param name="input"></param>
        /// <returns></returns>
        [DistributedLock("ModuleAppService_AddResource")]
        public async Task AddResource(ResourceAddInput input)
        {
            var parentNode = await GetModuleDetail(input.ParentId.Value);

            H_AssertEx.That(parentNode.Type != ModuleType.Sub, "非子菜单无法添加资源");

            var isExistSameName = await _moduleRep.IsExistSameNameModule(input.Name, ModuleType.Resource, input.ParentId);

            H_AssertEx.That(isExistSameName, "存在相同名称的资源，请重新输入");

            var isExistSameAlias = await _moduleRep.IsExistSameAliasModule(input.Alias, ModuleType.Resource, input.ParentId);

            H_AssertEx.That(isExistSameAlias, "存在相同别名的资源，请重新输入");

            var module = input.Adapt<SysModule>();
            module.Type = ModuleType.Resource;
            module.Sort = 0;
            module.ParentAlias = parentNode.Alias;

            await AddModule(module);
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteResource(long id)
        {
            H_AssertEx.That(id == 0, "无法操作系统根节点");

            var node = await GetModuleDetail(id);

            H_AssertEx.That(node.Type != ModuleType.Resource, "非资源项无法删除");

            await _moduleRep.DeleteAsync(node);
        }

        /// <summary>
        /// 资源列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResourceItemVM>> GetResourceList(long parentId)
        {
            var query = new ModuleQuery { ParentId = parentId };

            query.OrderBy(a => a.Sort).OrderBy(a => a.CreateTime);

            var resources = await _moduleRep.GetListAsync(query);

            var result = resources.Adapt<List<ResourceItemVM>>();
            return result;
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [DistributedLock("ModuleAppService_UpdateResource")]
        [UnitOfWork]
        public async Task UpdateResource(long id, ResourceUpdateInput vm)
        {
            H_AssertEx.That(id == 0, "无法操作系统根节点");

            var module = await GetModuleDetail(id);

            H_AssertEx.That(module.Type != ModuleType.Resource, "非资源项无法更新");

            var isExistSameName = await _moduleRep.IsExistSameNameModule(vm.Name, ModuleType.Resource, module.ParentId, id);

            H_AssertEx.That(isExistSameName, "存在相同名称的资源，请重新输入");

            var isExistSameAlias = await _moduleRep.IsExistSameAliasModule(vm.Alias, ModuleType.Resource, module.ParentId, id);

            H_AssertEx.That(isExistSameAlias, "存在相同别名的资源，请重新输入");

            module = vm.Adapt(module);
            await _moduleRep.UpdateAsync(module, a => new { a.Name, a.Sort, a.Alias });
        }
    }
}
