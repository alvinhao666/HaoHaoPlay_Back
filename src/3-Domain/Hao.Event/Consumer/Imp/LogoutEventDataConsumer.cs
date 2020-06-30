using Hao.Core;
using Hao.EventData;
using Hao.Json;
using Hao.Library;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Hao.Event
{
    /// <summary>
    /// 注销
    /// </summary>
    public class LogoutEventDataConsumer : EventService, ILogoutEventDataConsumer
    {
        private readonly H_AppSettingsConfig _appsettings;

        public LogoutEventDataConsumer(IOptionsSnapshot<H_AppSettingsConfig> appsettingsOptions)
        {
            _appsettings = appsettingsOptions.Value;
        }

        public async Task Logout(LogoutEventData person)
        {
            foreach(var userId in person.UserIds)
            {
                var keys = await RedisHelper.KeysAsync($"{_appsettings.RedisPrefix.Login}{userId}_*");
                foreach (var key in keys)
                {
                    var value = await RedisHelper.GetAsync(key);
                    var cacheUser = H_JsonSerializer.Deserialize<H_RedisCacheUser>(value);
                    cacheUser.IsAuthUpdate = true;
                    cacheUser.LoginStatus = LoginStatus.Offline;
                    await RedisHelper.SetAsync(key, H_JsonSerializer.Serialize(cacheUser));
                }
            }
        }
    }
}
