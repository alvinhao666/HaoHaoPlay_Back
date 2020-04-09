using Hao.Core;
using Hao.Library;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class LogoutAppService : ApplicationService, ILogoutAppService
    {
        private readonly AppSettingsInfo _appsettings;

        public LogoutAppService(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions)
        {
            _appsettings = appsettingsOptions.Value;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Logout(long userId)
        {
            await RedisHelper.DelAsync(_appsettings.RedisPrefixOptions.LoginInfo + userId);
        }
    }
}
