using Hao.Core;
using Hao.Enum;
using Hao.Model;
using Hao.Runtime;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 角色领域服务
    /// </summary>
    public class RoleDomainService : DomainService, IRoleDomainService
    {
        private readonly IRoleRepository _roleRep;

        private readonly ICurrentUser _currentUser;

        public RoleDomainService(IRoleRepository roleRep, ICurrentUser currentUser)
        {
            _roleRep = roleRep;
            _currentUser = currentUser;
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

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task UpdateRoleAuth(SysRole role)
        {
            if (role.Level != (int)RoleLevel.SuperAdministrator && _currentUser.RoleLevel >= role.Level) throw new H_Exception("无法操作该角色的权限");

            await _roleRep.UpdateAsync(role, a => new { a.AuthNumbers });
        }
    }
}
