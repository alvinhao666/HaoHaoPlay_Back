using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Enum;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public partial class ModuleAppService
    {
        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddResource(ResourceAddRequest request)
        {
            var parentNode = await GetModuleDetail(request.ParentId.Value);
            if (parentNode.Type != ModuleType.Sub) throw new HException("非子菜单无法添加资源");
            var module = _mapper.Map<SysModule>(request);
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
            if (id == 0) throw new HException("无法操作系统根节点");

            var node = await GetModuleDetail(id);
            if (node.Type != ModuleType.Resource) throw new HException("非资源项无法删除");

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
                ParentId = parentId
            });

            var result = _mapper.Map<List<ResourceItemVM>>(resources);
            return result;
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateResource(long id, ResourceUpdateRequest request)
        {
            if (id == 0) throw new HException("无法操作系统根节点");
            var module = await GetModuleDetail(id);
            module.Name = request.Name;
            module.Sort = request.Sort;
            if (module.Type != ModuleType.Resource) throw new HException("非资源项无法更新");
            await _moduleRep.UpdateAsync(module, user => new { module.Name, module.Sort });
        }
    }
}
