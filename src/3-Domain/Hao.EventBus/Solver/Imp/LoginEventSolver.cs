using Hao.Core;
using Hao.EventData;
using Hao.Model;
using System.Threading.Tasks;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginEventSolver : EventSolver, ILoginEventSolver
    {
        private readonly IUserRepository _userRep;

        private readonly ILoginRecordRepository _recordRep;

        public LoginEventSolver(IUserRepository userRep, ILoginRecordRepository recordRep)
        {
            _userRep = userRep;
            _recordRep = recordRep;
        }

        [UnitOfWork]
        public async Task UpdateLogin(LoginEventData data)
        {
            var user = await _userRep.GetAsync(data.UserId.Value);
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

            await _recordRep.InsertAsync(record);
        }
    }
}
