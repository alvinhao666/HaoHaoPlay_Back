using Hao.Core;
using Hao.Library;
using Hao.Model;
using Npgsql;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 角色领域服务
    /// </summary>
    public class RoleService : DomainService, IRoleService
    {
        private readonly ISysRoleRepository _roleRep;

        public RoleService(ISysRoleRepository roleRep)
        {
            _roleRep = roleRep;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task Add(SysRole role)
        {
            try
            {
                await _roleRep.InsertAsync(role);
            }
            catch (PostgresException ex)
            {
                H_AssertEx.That(ex.SqlState == H_PostgresSqlState.E23505, "角色名称已存在，请重新输入");//违反唯一键
            }
        }
    }
}
