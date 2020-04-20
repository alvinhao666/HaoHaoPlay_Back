using System.Threading.Tasks;
using Hao.AppService.ViewModel;
using System.Collections.Generic;

namespace Hao.AppService
{
    public interface IRoleAppService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task AddRole(RoleAddRequest vm);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<RoleVM>>  GetRoleList();

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task UpdateRoleAuth(long id, RoleUpdateRequest vm);

        /// <summary>
        /// 获取角色拥有的模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<RoleModuleVM>> GetRoleModule(long id);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteRole(long id);
    }
}