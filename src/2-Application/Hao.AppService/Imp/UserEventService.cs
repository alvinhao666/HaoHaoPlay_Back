using Hao.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 处理用户消息事件的服务
    /// </summary>
    public partial class UserAppService
    {
        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastLoginTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task UpdateLogin(long userId, DateTime lastLoginTime, string ip)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user == null) return;
            user.LastLoginTime = lastLoginTime;
            user.LastLoginIP = ip;

            await _userRep.UpdateAsync(user, user => new { user.LastLoginTime, user.LastLoginIP });
            await _recordRep.InsertAysnc(new SysLoginRecord() { UserId = user.Id, IP = user.LastLoginIP, Time = user.LastLoginTime });
        }
    }
}
