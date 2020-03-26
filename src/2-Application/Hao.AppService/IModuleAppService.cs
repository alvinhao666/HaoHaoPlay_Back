using Hao.AppService.ViewModel;
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
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddModule(ModuleAddRequest request);

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
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateModule(long id, ModuleUpdateRequest request);


        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);

    }
}
