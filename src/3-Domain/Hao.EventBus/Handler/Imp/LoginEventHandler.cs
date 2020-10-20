using Hao.Core;
using Hao.EventData;
using Hao.Model;
using System.Threading.Tasks;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginEventHandler : EventHandleService, ILoginEventHandler
    {
        private readonly ISysUserRepository _userRep;

        private readonly ISysLoginRecordRepository _recordRep;

        public LoginEventHandler(ISysUserRepository userRep, ISysLoginRecordRepository recordRep)
        {
            _userRep = userRep;
            _recordRep = recordRep;
        }

        [UnitOfWork]
        public async Task UpdateLogin(LoginEventData data)
        {
            var user = await _userRep.GetAysnc(data.UserId.Value);
            if (user == null) return;
            user.LastLoginTime = data.LoginTime;
            user.LastLoginIP = data.LoginIP;

            await _userRep.UpdateAsync(user, user => new { user.LastLoginTime, user.LastLoginIP });


            var record = new SysLoginRecord();

            record.UserId = user.Id;
            record.IP = user.LastLoginIP;
            record.Time = user.LastLoginTime;
            record.JwtExpireTime = data.JwtExpireTime;
            record.JwtJti = data.JwtJti;

            await _recordRep.InsertAysnc(record);
        }
    }
}
