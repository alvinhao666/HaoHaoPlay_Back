using Hao.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Model
{
    /// <summary>
    /// 系统用户 abp框架 当在领域层中为定义了仓储接口，应该在基础设施层中实现这些接口
    /// </summary>
    public interface ISysUserRepository : IRepository<SysUser,long>
    {

        /// <summary>
        /// 根据登录名密码查询用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<List<SysUser>> GetUserByLoginName(string loginName, string password);

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authNumbers"></param>
        /// <returns></returns>
        Task UpdateAuth(long? roleId, string authNumbers);
    }
}
