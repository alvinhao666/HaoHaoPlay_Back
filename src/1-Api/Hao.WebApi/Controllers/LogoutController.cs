using Hao.AppService;
using Hao.Core;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 注销
    /// </summary>
    [Route("[controller]")]
    public class LogoutController : HController
    {
        private readonly ILogoutAppService _logoutAppService;

        public ICurrentUser CurrentUser { get; set; }

        public LogoutController(ILogoutAppService logoutAppService)
        {
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
