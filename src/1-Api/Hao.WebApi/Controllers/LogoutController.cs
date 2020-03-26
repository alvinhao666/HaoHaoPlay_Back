using Hao.Core;
using Hao.Core.Extensions;
using Hao.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    [Route("[controller]")]
    public class LogoutController : HController
    {

        private readonly AppSettingsInfo _appsettings;

        public ICurrentUser CurrentUser { get; set; }

        public LogoutController(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions)
        {
            _appsettings = appsettingsOptions.Value;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task Logout()
        {
            await RedisHelper.DelAsync(_appsettings.RedisPrefixOptions.LoginInfo + CurrentUser.Id);
        }
    }
}
