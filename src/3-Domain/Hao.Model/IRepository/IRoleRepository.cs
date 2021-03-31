using Hao.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Model
{
    public interface IRoleRepository: IRepository<SysRole, long>
    {
        Task<List<RoleDto>> GetRoleList(RoleQuery query);
    }
}