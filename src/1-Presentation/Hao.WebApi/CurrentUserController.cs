using Hao.AppService;
using Hao.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hao.WebApi
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUserController : H_Controller
    {
        private readonly ICurrentUserAppService _currentUserAppService;

        public CurrentUserController(ICurrentUserAppService currentUserAppService)
        {
            _currentUserAppService = currentUserAppService;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CurrentUserVM> GetUser() => await _currentUserAppService.Get();

        /// <summary>
        /// 更新当前用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateHeadImg([FromBody] UpdateHeadImgInput input) => await _currentUserAppService.UpdateHeadImg(input);


        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateBaseInfo([FromBody] CurrentUserUpdateInput input) => await _currentUserAppService.UpdateBaseInfo(input);


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
        public async Task UpdatePassword([FromBody] PwdUpdateInput input) => await _currentUserAppService.UpdatePassword(input.OldPassword, input.NewPassword);

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void Logout() => _currentUserAppService.Logout();
    }
}
