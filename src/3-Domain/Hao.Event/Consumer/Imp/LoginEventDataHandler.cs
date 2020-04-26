using Hao.Core;
using Hao.EventData;
using Hao.Model;
using Hao.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Event
{
    public class LoginEventDataHandler : HEventHandler, ILoginEventDataHandler
    {
        private readonly ISysUserRepository _userRep;

        private readonly ISysLoginRecordRepository _recordRep;

        public LoginEventDataHandler(ISysUserRepository userRep, ISysLoginRecordRepository recordRep)
        {
            _userRep = userRep;
            _recordRep = recordRep;
        }

        [UnitOfWork]
        public async Task UpdateLogin(LoginEventData person)
        {
            var user = await _userRep.GetAysnc(person.UserId.Value);
            if (user == null) return;
            user.LastLoginTime = person.LastLoginTime;
            user.LastLoginIP = person.LastLoginIP;

            await _userRep.UpdateAsync(user, user => new { user.LastLoginTime, user.LastLoginIP });
            await _recordRep.InsertAysnc(new SysLoginRecord() { UserId = user.Id, IP = user.LastLoginIP, Time = user.LastLoginTime });
        }
    }
}
