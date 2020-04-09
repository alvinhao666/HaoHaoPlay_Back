using Hao.AppService;
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

        private readonly ILogoutAppService _logoutAppService;

        public ICurrentUser CurrentUser { get; set; }

        public LogoutController(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, ILogoutAppService logoutAppService)
        {
            _appsettings = appsettingsOptions.Value;
            _logoutAppService = logoutAppService;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task Logout()
        {
            await _logoutAppService.Logout(CurrentUser.Id);
        }
    }
}
