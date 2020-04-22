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
    public partial class UserController : HController
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("Current")]
        public async Task<CurrentUserVM> GetCurrentUser() => await _userAppService.GetCurrent();

        /// <summary>
        /// 更新当前用户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateCurrentHeadImg")]
        public async Task UpdateCurrentHeadImg([FromBody]UpdateHeadImgRequest request)
        {
            await _userAppService.UpdateCurrentHeadImg(request);
        }

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateCurrentBaseInfo")]
        public async Task UpdateCurrentBaseInfo([FromBody]CurrentUserUpdateRequest request) => await _userAppService.UpdateCurrentBaseInfo(request);


        /// <summary>
        /// 当前用户安全信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("CurrentSecurityInfo")]
        public async Task<UserSecurityVM> GetCurrentSecurityInfo() => await _userAppService.GetCurrentSecurityInfo();


        /// <summary>
        /// 更新当前用户密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateCurrentPassword")]
        public async Task UpdateCurrentPassword([FromBody]PwdUpdateRequest request) => await _userAppService.UpdateCurrentPassword(request.OldPassword, request.NewPassword);
    }
}
