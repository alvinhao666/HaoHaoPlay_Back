using Hao.Core;
using Hao.EventData;
using Hao.Library;
using Hao.Model;
using Hao.Utility;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销
    /// </summary>
    public class LogoutEventHandler : EventHandleService, ILogoutEventHandler
    {
        private readonly ISysLoginRecordRepository _loginRecordRep;

        private readonly H_AppSettingsConfig _appsettings;

        public LogoutEventHandler(IOptionsSnapshot<H_AppSettingsConfig> appsettingsOptions, ISysLoginRecordRepository loginRecordRep)
        {
            _appsettings = appsettingsOptions.Value;
            _loginRecordRep = loginRecordRep;
        }

        public async Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data)
        {
            foreach (var userId in data.UserIds)
            {
                //var keys = await RedisHelper.KeysAsync($"{_appsettings.RedisPrefix.Login}{userId}_*"); //不会自动加prefix

                //一般在测试环境中，可以使用keys命令，模糊查询到需要的key，但这个操作只适合在测试环境中使用，不适合在生产环境中使用
                //原因是redis是单线程运行的，当redis中的数据量很大时，由于此操作会遍历所有数据，并将结果一次性全部返回，执行时间会比较长，从而导致后续操作等待，直接影响系统的正常运行
                
                var records = await _loginRecordRep.GetLoginRecords(userId, data.TimeNow);

                foreach(var item in records)
                {
                    var key = $"{_appsettings.RedisPrefix.Login}{userId}_{item.JwtJti}";
                    var value = await RedisHelper.GetAsync(key);
                    if (value.IsNullOrWhiteSpace()) continue;

                    var cacheUser = H_JsonSerializer.Deserialize<H_CacheUser>(value);
                    cacheUser.IsAuthUpdate = true;
                    cacheUser.LoginStatus = LoginStatus.Offline;
                    await RedisHelper.SetAsync(key, H_JsonSerializer.Serialize(cacheUser), true); //false 失效 ttl: -1  true:继续保持原先的time
                }

            }
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Logout(LogoutEventData data)
        {
            foreach (var userId in data.UserIds)
            {
                var records = await _loginRecordRep.GetLoginRecords(userId, data.TimeNow);

                foreach (var item in records)
                {
                    var key = $"{_appsettings.RedisPrefix.Login}{userId}_{item.JwtJti}";
                    await RedisHelper.DelAsync(key);
                }
            }
        }
    }
}
