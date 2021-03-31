using Hao.Core;
using Hao.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class RoleRepository : Repository<SysRole, long>, IRoleRepository
    {
        public async Task<List<RoleDto>> GetRoleList(RoleQuery query)
        {
            var roles = await DbContext.Select<SysRole>()
                        .WhereIf(query.CurrentRoleLevel.HasValue, x => x.Level > query.CurrentRoleLevel)
                        .OrderBy(a => a.Level)
                        .ToListAsync(a => new RoleDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            UserCount = (int)DbContext.Select<SysUser>().Where(b => b.RoleLevel == a.Level && b.Enabled == true).Count()
                        });

            return roles;
        }
    }
}