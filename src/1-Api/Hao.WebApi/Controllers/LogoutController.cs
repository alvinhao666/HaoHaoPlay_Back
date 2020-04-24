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

        private readonly ICurrentUser _currentUser;

        public LogoutController(ILogoutAppService logoutAppService, ICurrentUser currentUser)
        {
            _logoutAppService = logoutAppService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task Logout()
        {
            await _logoutAppService.Logout(_currentUser.Id.Value, _currentUser.Jti);
        }
    }
}
