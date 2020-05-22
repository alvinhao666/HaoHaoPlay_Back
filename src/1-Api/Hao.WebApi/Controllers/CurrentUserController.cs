using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUserController : H_Controller
    {
        private readonly ICurrentUserAppService _currentUserAppService;

        private readonly ILogoutAppService _logoutAppService;

        private readonly ICurrentUser _currentUser;

        public CurrentUserController(ICurrentUserAppService currentUserAppService, ILogoutAppService logoutAppService, ICurrentUser currentUser)
        {
            _currentUserAppService = currentUserAppService;
            _logoutAppService = logoutAppService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CurrentUserVM> GetUser() => await _currentUserAppService.GetUser();

        /// <summary>
        /// 更新当前用户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateHeadImg([FromBody]UpdateHeadImgRequest request) => await _currentUserAppService.UpdateHeadImg(request);


        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateBaseInfo([FromBody]CurrentUserUpdateRequest request) => await _currentUserAppService.UpdateBaseInfo(request);


        /// <summary>
        /// 当前用户安全信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserSecurityVM> SecurityInfo() => await _currentUserAppService.GetSecurityInfo();


        /// <summary>
        /// 更新当前用户密码
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdatePassword([FromBody]PwdUpdateRequest request) => await _currentUserAppService.UpdatePassword(request.OldPassword, request.NewPassword);

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task Logout() => await _logoutAppService.Logout(_currentUser.Id.Value, _currentUser.Jti);
    }
}
