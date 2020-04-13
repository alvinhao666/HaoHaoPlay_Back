using Hao.Core;
using Hao.Model;
using Hao.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class RoleService : ApplicationService, IRoleService
    {
        private readonly ISysRoleRepository _roleRep;


        private readonly ISysUserRepository _userRep;

        public RoleService(ISysRoleRepository roleRep,ISysUserRepository userRep)
        {
            _roleRep = roleRep;
            _userRep = userRep;
        }

        [UseTransaction]//注意，事务命令只能用于 insert、delete、update 操作，而其他命令，比如建表、删表，会被自动提交。
        public async Task UpdateAuth(SysRole role)
        {
            await _roleRep.UpdateAsync(role, a => new { a.AuthNumbers });
            await _userRep.UpdateAuth(role.Id, role.AuthNumbers);
        }
    }
}
