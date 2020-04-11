using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Model;

namespace Hao.Repository
{
    public class SysRoleRepository: Repository<SysRole, long>, ISysRoleRepository
    {
        /// <summary>
        /// 获取角色对应的角色数量
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleUserCountDto>> GetRoleUserCount()
        {
            var result = await UnitOfWork.GetDbClient().SqlQueryable<RoleUserCountDto>("select roleid,count(*) as usercount from sysuser group by roleid ").ToListAsync();
            return result;
        }
    }
}