using Hao.Core;
using Hao.Model;
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
