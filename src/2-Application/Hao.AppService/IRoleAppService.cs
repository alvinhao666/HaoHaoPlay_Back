using System.Threading.Tasks;
using System.Collections.Generic;

namespace Hao.AppService
{
    /// <summary>
    /// 角色服务接口
    /// </summary>
    public interface IRoleAppService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Add(RoleAddInput input);

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<RoleVM>> GetList();

        /// <summary>
        /// 根据当前用户角色，获取可以操作得角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<RoleSelectVM>> GetRoleListByCurrentRole();

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateRoleAuth(long id, RoleUpdateInput input);

        /// <summary>
        /// 获取角色拥有的模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RoleModuleVM> GetRoleModule(long id);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);
    }
}