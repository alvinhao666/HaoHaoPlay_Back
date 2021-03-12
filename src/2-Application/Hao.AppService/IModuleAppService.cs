using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 模块服务接口
    /// </summary>
    public interface IModuleAppService
    {
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Add(ModuleAddInput input);

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);

        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Update(long id, ModuleUpdateInput input);

        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        Task<List<ModuleTreeOutput>> GetTreeList();

        /// <summary>
        /// 获取模块详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ModuleDetailOutput> Get(long id);

        #region 资源
        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task AddResource(ResourceAddInput vm);

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteResource(long id);

        /// <summary>
        /// 更新资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task UpdateResource(long id, ResourceUpdateInput vm);

        /// <summary>
        /// 获取资源列表
        /// </summary>
        /// <returns></returns>
        Task<List<ResourceItemOutput>> GetResourceList(long parentId);

        #endregion

    }
}
