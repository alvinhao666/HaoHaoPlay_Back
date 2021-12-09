using Hao.Core;
using Hao.Library;
using Hao.Model;
using Hao.Utility;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using Hao.Runtime;
using Mapster;
using Hao.Redis;
using Hao.Service;

namespace Hao.AppService
{
    /// <summary>
    /// 当前用户应用服务
    /// </summary>
    public class CurrentUserAppService : ApplicationService, ICurrentUserAppService
    {
        private readonly IUserRepository _userRep;

        private readonly ICurrentUser _currentUser;

        private readonly IUserDomainService _userDomainService;

        private readonly AppSettings _appSettings;


        public CurrentUserAppService(IUserRepository userRepository, ICurrentUser currentUser, IUserDomainService userDomainService,
            IOptionsSnapshot<AppSettings> appSettingsOptions)
        {
            _userRep = userRepository;
            _currentUser = currentUser;
            _userDomainService = userDomainService;
            _appSettings = appSettingsOptions.Value;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUserOutput> Get()
        {
            var user = await _userDomainService.Get(_currentUser.Id.Value);
            return user.Adapt<CurrentUserOutput>();
        }

        // /// <summary>
        // /// 更新头像地址 (ImageSharp)
        // /// </summary>
        // /// <param name="input"></param>
        // /// <returns></returns>
        // public async Task UpdateHeadImg(UpdateHeadImgRequest input)
        // {
        //     string[] str = input.Base64Str.Split(','); //base64Str为base64完整的字符串，先处理一下得到我们所需要的字符串
        //     if (str.Length < 2) throw new H_Exception("图片格式不对");
        //     byte[] imageBytes = Convert.FromBase64String(str[1]);
        //
        //     H_File.CreateDirectory(_appSettings.FilePath.AvatarPath);
        //     string imgName = $"{_currentUser.Id}_{H_Util.GetUnixTimestamp()}.png";
        //     string imgPath = Path.Combine(_appSettings.FilePath.AvatarPath, imgName);
        //     using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(imageBytes))
        //     {
        //         image.Save(imgPath);
        //     }
        //
        //     var user = await _userRep.GetAsync(_currentUser.Id.Value);
        //     string oldImgUrl = user.HeadImgUrl;
        //     user.HeadImgUrl = imgName;
        //     await _userRep.UpdateAsync(user, user => new { user.HeadImgUrl });
        //     if (!string.IsNullOrWhiteSpace(oldImgUrl))
        //     {
        //         H_File.DeleteFile(Path.Combine(_appSettings.FilePath.AvatarPath, oldImgUrl));
        //     }
        // }

        /// <summary>
        /// 更新头像地址 (腾讯COS)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateHeadImg(UpdateHeadImgInput input)
        {
            var user = await _userDomainService.Get(_currentUser.Id.Value);
            user.HeadImgUrl = $"https://{input.HeadImageUrl}";
            await _userRep.UpdateAsync(user, user => new { user.HeadImgUrl });
        }

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateBaseInfo(CurrentUserUpdateInput input)
        {
            var user = await _userDomainService.Get(_currentUser.Id.Value);

            user = input.Adapt(user);

            await _userRep.UpdateAsync(user,
                user => new
                {
                    user.Phone,
                    user.WeChat,
                    user.Profile,
                    user.HomeAddress,
                });
        }

        /// <summary>
        /// 更新当前用户密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task UpdatePassword(string oldPassword, string newPassword)
        {
            await _userDomainService.UpdatePwd(_currentUser.Id.Value, oldPassword, newPassword);
        }

        /// <summary>
        /// 当前用户安全信息
        /// </summary>
        public async Task<UserSecurityOutput> GetSecurityInfo()
        {
            var user = await _userDomainService.Get(_currentUser.Id.Value);
            var result = user.Adapt<UserSecurityOutput>();
            return result;
        }


        /// <summary>
        /// 注销当前登录
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            var key = $"{_appSettings.RedisPrefix.Login}{_currentUser.Id.Value}_{_currentUser.Jti}";

            var value = RedisHelper.Cli.Get(key);

            if (value.HasValue()) RedisHelper.Cli.Del(key);
        }
    }
}