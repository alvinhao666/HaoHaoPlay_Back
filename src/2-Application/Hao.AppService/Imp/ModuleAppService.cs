using Hao.Core;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hao.Enum;
using Npgsql;
using Hao.Library;
using Mapster;

namespace Hao.AppService
{
    /// <summary>
    /// 模块应用服务
    /// </summary>
    public partial class ModuleAppService : ApplicationService, IModuleAppService
    {
        private readonly ISysModuleRepository _moduleRep;

        public ModuleAppService(ISysModuleRepository moduleRep)
        {
            _moduleRep = moduleRep;
        }


        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [DistributedLock("ModuleAppService_AddModule")]
        public async Task Add(ModuleAddRequest vm)
        {
            var parentNode = await GetModuleDetail(vm.ParentId.Value);

            H_AssertEx.That(parentNode.Type == ModuleType.Sub, "子菜单无法继续添加节点");

            var isExistSameName = await _moduleRep.IsExistSameNameModule(vm.Name, vm.Type, vm.ParentId);

            H_AssertEx.That(isExistSameName, "存在相同名称的模块，请重新输入");

            var module = vm.Adapt<SysModule>();

            if (parentNode.Type == ModuleType.Sub) module.Alias = $"{parentNode.Alias}_{module.Alias}";

            module.ParentAlias = parentNode.Alias;

            await AddModule(module);
        }

        /// <summary>
        /// 获取所有模块列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModuleVM>> GetTreeList()
        {
            var modules = await _moduleRep.GetListAysnc(new ModuleQuery() { IncludeResource = false });
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
            var result = module.Adapt<ModuleDetailVM>();

            if (result.Type == ModuleType.Sub)
            {
                var query = new ModuleQuery { ParentId = id };
                
                query.OrderBy(a=>a.Sort).OrderBy(a=>a.CreateTime);

                var resources = await _moduleRep.GetListAysnc(query);

                result.Resources = resources.Adapt<List<ResourceItemVM>>();
            }

            return result;
        }

        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [DistributedLock("ModuleAppService_UpdateModule")]
        public async Task Update(long id, ModuleUpdateRequest vm)
        {
            H_AssertEx.That(id == 0, "无法操作系统根节点");

            var module = await GetModuleDetail(id);

            var isExistSameName = await _moduleRep.IsExistSameNameModule(vm.Name, module.Type, module.ParentId, id);

            H_AssertEx.That(isExistSameName, "存在相同名称的模块，请重新输入");

            module.Name = vm.Name;
            module.Sort = vm.Sort;
            if (module.Type == ModuleType.Main)
            {
                module.Icon = vm.Icon;
                await _moduleRep.UpdateAsync(module, user => new { module.Name, module.Icon, module.Sort });
            }
            else if (module.Type == ModuleType.Sub)
            {
                module.RouterUrl = vm.RouterUrl;
                await _moduleRep.UpdateAsync(module, user => new { module.Name, module.RouterUrl, module.Sort });
            }
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            H_AssertEx.That(id == 0, "无法操作系统根节点");

            var module = await _moduleRep.GetAysnc(id);

            var childs = await _moduleRep.GetListAysnc(new ModuleQuery()
            {
                ParentId = module.Id
            });

            H_AssertEx.That(childs != null && childs.Count > 0, "存在子节点无法删除");

            await _moduleRep.DeleteAysnc(module);
        }


        #region private

        private async Task AddModule(SysModule module)
        {
            var max = await _moduleRep.GetLayerCount();
            if (max.Count < 31)
            {
                module.Layer = max.Layer;
                module.Number = Convert.ToInt64(Math.Pow(2, max.Count.Value));
            }
            else if (max.Count == 31) //0次方 到 30次方 共31个数              js语言的限制 导致  位运算 32位  
            {
                module.Layer = ++max.Layer;
                module.Number = 1;
            }
            else
            {
                throw new H_Exception("数据库数据异常，请检查");
            }

            try
            {
                await _moduleRep.InsertAysnc(module);
            }
            catch (PostgresException ex)
            {
                H_AssertEx.That(ex.SqlState == H_PostgresSqlState.E23505, "添加失败，请重新添加");
            }
        }

        /// <summary>
        /// 递归初始化模块树
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parentID"></param>
        /// <param name="sources"></param>
        private void InitModuleTree(List<ModuleVM> result, long? parentID, List<SysModule> sources)
        {
            //递归寻找子节点  
            var tempTree = sources.Where(item => item.ParentId == parentID).OrderBy(a => a.Sort);
            foreach (var item in tempTree)
            {
                var node = new ModuleVM()
                {
                    key = item.Id.ToString(),
                    title = item.Name,
                    isLeaf = item.Type == ModuleType.Sub,
                    expanded = true,
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

            H_AssertEx.That(module == null, "节点不存在");
            H_AssertEx.That(module.IsDeleted, "节点已删除");

            return module;
        }

        #endregion
    }
}