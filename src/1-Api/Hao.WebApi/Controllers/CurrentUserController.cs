using AutoMapper;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 当前用户
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CurrentUserController : HController
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
        public async Task<CurrentUserVM> GetUser() => await _currentUserAppService.GetUser();

        /// <summary>
        /// 更新当前用户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateHeadImg")]
        public async Task UpdateCurrentHeadImg([FromBody]UpdateHeadImgRequest request) => await _currentUserAppService.UpdateHeadImg(request);


        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateBaseInfo")]
        public async Task UpdateCurrentBaseInfo([FromBody]CurrentUserUpdateRequest request) => await _currentUserAppService.UpdateBaseInfo(request);


        /// <summary>
        /// 当前用户安全信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("SecurityInfo")]
        public async Task<UserSecurityVM> GetCurrentSecurityInfo() => await _currentUserAppService.GetSecurityInfo();


        /// <summary>
        /// 更新当前用户密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdatePassword")]
        public async Task UpdateCurrentPassword([FromBody]PwdUpdateRequest request) => await _currentUserAppService.UpdatePassword(request.OldPassword, request.NewPassword);
    }
}
