using Hao.Core;
using Hao.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysUserRepository : Repository<SysUser, long>, ISysUserRepository
    {

        /// <summary>
        /// 根据登录名密码查询用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<List<SysUser>> GetUserByLoginName(string loginName, string password)
        {
            var result = await DbContext.Select<SysUser>()
                                 .Where(a => a.LoginName == loginName)
                                 .Where(a => a.Password == password)
                                 .ToListAsync();

            return result;
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authNumbers"></param>
        /// <returns></returns>
        public async Task UpdateAuth(long? roleId, string authNumbers)
        {
            await DbContext.Update<SysUser>()
                    .Set(a => a.AuthNumbers, authNumbers)
                    .WhereIf(roleId.HasValue, a => a.RoleId == roleId)
                    .ExecuteAffrowsAsync();
        }

    }
}
