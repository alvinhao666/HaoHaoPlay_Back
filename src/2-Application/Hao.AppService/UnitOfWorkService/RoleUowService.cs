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
        /// PostgreSQL不支持嵌套或并发事务-在任何给定时刻，可能只有一个事务正在进行。在事务正在进行时调用BeginTransaction（）将引发异常。
        /// 因此，不必将从BeginTransaction（）返回的NpgsqlTransaction对象传递给您执行的命令-调用BeginTransaction（）意味着在执行提交或回滚之前，所有后续命令都将自动参与事务。
        /// 但是，为了获得最大的可移植性，建议在命令上设置事务。
        /// 虽然不支持并发事务，但PostgreSQL支持保存点的概念-您可以在事务中设置命名的保存点，然后回滚到它们，而不回滚整个事务。保存点可以通过NpgsqlTransaction.Save（name）创建、回滚到并释放
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
