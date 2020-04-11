using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Model;

namespace Hao.Repository
{
    public interface ISysRoleRepository: IRepository<SysRole, long>
    {
        /// <summary>
        /// 获取角色对应的用户数量
        /// </summary>
        /// <returns></returns>
        Task<List<RoleUserCountDto>> GetRoleUserCount();
    }
}