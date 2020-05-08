using Hao.Core;
using Hao.Library;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 注销应用服务
    /// </summary>
    public class LogoutAppService : ApplicationService, ILogoutAppService
    {
        private readonly AppSettingsInfo _appsettings;

        public LogoutAppService(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions)
        {
            _appsettings = appsettingsOptions.Value;
        }

        /// <summary>
        /// 注销当前登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jti"></param>
        /// <returns></returns>
        public async Task Logout(long userId, string jti)
        {
            var value = await RedisHelper.GetAsync($"{_appsettings.RedisPrefixOptions.LoginInfo}{userId}_{jti}");
            var cacheUser = JsonSerializer.Deserialize<RedisCacheUserInfo>(value);
            cacheUser.LoginStatus = LoginStatus.Offline;
            await RedisHelper.SetAsync($"{_appsettings.RedisPrefixOptions.LoginInfo}{userId}_{jti}",JsonSerializer.Serialize(cacheUser));
        }
    }
}
