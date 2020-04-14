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
        /// 更新权限 
        /// postgresql 事务操作中不能出现异步线程操作数据库；否则会报错  Npgsql.NpgsqlOperationInProgressException: A command is already in progress
        /// 但连接仍忙于第一个查询（演示实体）的结果。您实际上正在尝试在同一连接上同时执行两个数据库命令，该功能有时称为多个活动结果集，Npgsql/PostgreSQL 不支持此功能。
        /// </summary>
        /// <param name="role"></param>
        [UseTransaction]
        public void UpdateAuth(SysRole role)
        {
             _roleRep.UpdateAsync(role, a => new { a.AuthNumbers });
             _userRep.UpdateAuth(role.Id, role.AuthNumbers);
        }

    }
}
