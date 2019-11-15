using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.Core;
using Hao.EventData;
using Hao.Model;
using Hao.Repository;

namespace Hao.Event
{
    public class LoginEventHandler : HEventHandler, ILoginEventHandler, ICapSubscribe
    {

        private readonly ISysUserRepository _userRep;

        private readonly ISysLoginRecordRepository _recordRep;

        public LoginEventHandler(ISysUserRepository userRep, ISysLoginRecordRepository recordRep)
        {
            _userRep = userRep;
            _recordRep = recordRep;
        }

        [CapSubscribe(nameof(LoginEventData))]
        [UseTransaction]
        public async Task UpdateLogin(LoginEventData person)
        {
            var user = await _userRep.GetAysnc(person.UserId);
            if (user != null)
            {
                user.LastLoginTime = person.LastLoginTime;
                user.LastLoginIP = person.LastLoginIP;
                await _userRep.UpdateAsync(user);
                await _recordRep.InsertAysnc(new SysLoginRecord() { UserId = user.Id, IP = person.LastLoginIP, Time = person.LastLoginTime });
            }
        }
    }
}
