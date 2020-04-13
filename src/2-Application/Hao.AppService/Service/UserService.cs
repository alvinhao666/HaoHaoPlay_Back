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
        /// //注意，事务命令只能用于 insert、delete、update 操作，而其他命令，比如建表、删表，会被自动提交。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lastLoginTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [UseTransaction]
        public async Task UpdateLoginWithTransacition(SysUser user, DateTime lastLoginTime, string ip)
        {
            if (user != null)
            {

                await _userRep.UpdateAsync(user, user => new { user.LastLoginTime, user.LastLoginIP });
                await _recordRep.InsertAysnc(new SysLoginRecord() { UserId = user.Id, IP = ip, Time = lastLoginTime });
            }
        }
    }
}
