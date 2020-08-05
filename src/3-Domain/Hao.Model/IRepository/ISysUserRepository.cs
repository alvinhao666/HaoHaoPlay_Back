using Hao.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Model
{
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
        Task UpdateAuth(long roleId, string authNumbers);
    }
}
