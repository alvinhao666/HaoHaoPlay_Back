using Hao.AppService.ViewModel;
using Hao.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IModuleAppService
    {
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task AddModule(ModuleAddRequest vm);

        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        Task<List<ModuleVM>> GetList();


        /// <summary>
        /// 获取模块详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ModuleDetailVM> Get(long id);


        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task UpdateModule(long id, ModuleUpdateRequest vm);


        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);


        #region 资源
        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task AddResource(ResourceAddRequest vm);

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteResource(long id);

        /// <summary>
        /// 获取资源列表
        /// </summary>
        /// <returns></returns>
        Task<List<ResourceItemVM>> GetResourceList(long parentId);

        /// <summary>
        /// 更新资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task UpdateResource(long id, ResourceUpdateRequest vm);
        #endregion

    }
}
