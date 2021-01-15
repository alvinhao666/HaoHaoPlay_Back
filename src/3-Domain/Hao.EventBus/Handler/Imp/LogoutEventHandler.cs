using Hao.Core;
using Hao.EventData;
using Hao.Library;
using Hao.Model;
using Hao.Utility;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
                
                var records = await _loginRecordRep.GetLoginRecords(userId, data.PublishTime);

                foreach(var item in records)
                {
                    var key = $"{_appsettings.RedisPrefix.Login}{userId}_{item.JwtJti}";
                    var value = RedisHelper.Get(key);
                    if (value.IsNullOrWhiteSpace()) continue;

                    var cacheUser = JsonConvert.DeserializeObject<H_CacheUser>(value);
                    cacheUser.IsAuthUpdate = true;
                    cacheUser.LoginStatus = LoginStatus.Offline;

                    //不设置expire ttl 返回-1, 表示永久存在
                    //设置了expire ttl 会返回剩余时间
                    //如果没有该键(改键从未设定过 ; 到了过期时间,被删除掉了) 直接返回 -2

                    var expireTime = RedisHelper.Ttl(key);

                    RedisHelper.Set(key, JsonConvert.SerializeObject(cacheUser), (int)expireTime); //false 失效 ttl: -1  true:继续保持原先的time，redis6.0.0才有效
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
                var records = await _loginRecordRep.GetLoginRecords(userId, data.PublishTime);

                foreach (var item in records)
                {
                    var key = $"{_appsettings.RedisPrefix.Login}{userId}_{item.JwtJti}";
                    RedisHelper.Del(key);
                }
            }
        }
    }
}
