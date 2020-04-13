using Hao.Core;
using Hao.Model;
using Hao.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    internal class UserService : ApplicationService, IUserService
    {

        private readonly ISysUserRepository _userRep;

        private readonly ISysLoginRecordRepository _recordRep;

        public UserService( ISysUserRepository userRepository, ISysLoginRecordRepository recordRep)
        {
            _userRep = userRepository;
            _recordRep = recordRep;
        }


        /// <summary>
        /// 更新登录信息    //注意，事务命令只能用于 insert、delete、update 操作，而其他命令，比如建表、删表，会被自动提交。//如果多个线程共享一个数据库连接，您必须同步它们，以便只有一个线程同时使用该连接。在 PostgreSQL 数据库会话中，一次只能运行一个语句。
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [UseTransaction]
        public void UpdateLogin(SysUser user)
        {
            _userRep.Update(user, user => new { user.LastLoginTime, user.LastLoginIP });
            _recordRep.Insert(new SysLoginRecord() { UserId = user.Id, IP = user.LastLoginIP, Time = user.LastLoginTime });
        }
    }
}
