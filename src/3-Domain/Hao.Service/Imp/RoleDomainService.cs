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
    public class RoleDomainService : DomainService, IRoleDomainService
    {
        private readonly IRoleRepository _roleRep;

        public RoleDomainService(IRoleRepository roleRep)
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

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<SysRole> Get(long roleId)
        {
            var item = await _roleRep.GetAsync(roleId);

            H_AssertEx.That(item == null, "角色不存在");
            H_AssertEx.That(item.IsDeleted, "角色已删除");

            return item;
        }
    }
}
