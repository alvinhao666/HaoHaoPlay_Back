using Hao.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Model
{
    public interface ISysRoleRepository: IRepository<SysRole, long>
    {
        Task<List<RoleDto>> GetRoleList(RoleQuery query);
    }
}