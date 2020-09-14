using Hao.Core;
using Hao.EventData;
using Hao.Model;
using System.Threading.Tasks;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginEventHandler : EventBusService, ILoginEventHandler
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
            user.LastLoginTime = data.LastLoginTime;
            user.LastLoginIP = data.LastLoginIP;

            await _userRep.UpdateAsync(user, user => new { user.LastLoginTime, user.LastLoginIP });
            await _recordRep.InsertAysnc(new SysLoginRecord() { UserId = user.Id, IP = user.LastLoginIP, Time = user.LastLoginTime });
        }
    }
}
