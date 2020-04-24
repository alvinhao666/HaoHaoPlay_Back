using Hao.Core;
using Hao.EventData;
using Hao.Library;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.Event
{
    public class LogoutEventDataHandler : HEventHandler, ILogoutEventDataHandler
    {
        private readonly AppSettingsInfo _appsettings;

        public LogoutEventDataHandler(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions)
        {
            _appsettings = appsettingsOptions.Value;
        }

        public async Task Logout(LogoutEventData person)
        {
            foreach(var userId in person.UserIds)
            {
                var keys = await RedisHelper.KeysAsync($"{_appsettings.RedisPrefixOptions.LoginInfo}{userId}_*");
                foreach (var key in keys)
                {
                    var value = await RedisHelper.GetAsync(key);
                    var cacheUser = JsonSerializer.Deserialize<RedisCacheUserInfo>(value);
                    cacheUser.IsAuthUpdate = true;
                    cacheUser.LoginStatus = LoginStatus.Offline;
                    await RedisHelper.SetAsync(key, JsonSerializer.Serialize(cacheUser));
                }
            }
        }
    }
}
