using Hao.Core;
using Hao.Model;
using Hao.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class RoleUowService : UnitOfWorkService, IRoleUowService
    {
        private readonly ISysRoleRepository _roleRep;


        private readonly ISysUserRepository _userRep;

        public RoleUowService(ISysRoleRepository roleRep,ISysUserRepository userRep)
        {
            _roleRep = roleRep;
            _userRep = userRep;
        }

        /// <summary>
        /// 更新权限 //postgresql 事务操作放一个线程中；否则会报错
        /// </summary>
        /// <param name="role"></param>
        [UseTransaction]
        public void UpdateAuth(SysRole role)
        {
             _roleRep.Update(role, a => new { a.AuthNumbers });
             _userRep.UpdateAuth(role.Id, role.AuthNumbers);
        }
    }
}
