using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Model;

namespace Hao.Repository
{
    public class SysRoleRepository: Repository<SysRole, long>, ISysRoleRepository
    {
        private readonly ISysUserRepository _userRep;

        public SysRoleRepository(ISysUserRepository userRep)
        {
            _userRep = userRep;
        }

        /// <summary>
        /// 获取角色对应的角色数量
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleUserCountDto>> GetRoleUserCount()
        {
            string sql = "select roleid,count(*) as usercount from sysuser where isdeleted=false group by roleid ";
            var result = await Db.SqlQueryable<RoleUserCountDto>(sql).ToListAsync();
            return result;
        }

    }
}