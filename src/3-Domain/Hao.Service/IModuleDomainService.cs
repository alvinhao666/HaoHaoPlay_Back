using Hao.Enum;
using Hao.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 菜单模块领域服务接口
    /// </summary>
    public interface IModuleDomainService
    {
        /// <summary>
        /// 添加菜单模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        Task Add(SysModule module);


        /// <summary>
        /// 获取菜单模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysModule> Get(long id);

        /// <summary>
        /// 是否存在相同名字的模块
        /// </summary>
        Task IsExistSameName(string name, ModuleType? moduleType, long? parentId, long? id = null);

        /// <summary>
        /// 是否存在相同别名的模块
        /// </summary>
        Task IsExistSameAlias(string alias, ModuleType? moduleType, long? parentId, long? id = null);


        /// <summary>
        /// 获取权限数组值对应的应用菜单树
        /// </summary>
        /// <param name="authNums"></param>
        /// <returns></returns>
        Task<List<MenuTreeDto>> GetMenuTree(List<long> authNums);
    }
}
