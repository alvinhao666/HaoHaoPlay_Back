using Hao.Core;
using Hao.EventData;
using Hao.Library;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.Event
{
    /// <summary>
    /// 注销
    /// </summary>
    public class LogoutEventDataConsumer : EventConsumer, ILogoutEventDataConsumer
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
                    var cacheUser = JsonSerializer.Deserialize<RedisCacheUser>(value);
                    cacheUser.IsAuthUpdate = true;
                    cacheUser.LoginStatus = LoginStatus.Offline;
                    await RedisHelper.SetAsync(key, JsonSerializer.Serialize(cacheUser));
                }
            }
        }
    }
}
