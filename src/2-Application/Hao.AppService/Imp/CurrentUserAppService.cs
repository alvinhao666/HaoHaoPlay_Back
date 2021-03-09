using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
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

namespace Hao.AppService
{
    /// <summary>
    /// 当前用户应用服务
    /// </summary>
    public class CurrentUserAppService : ApplicationService, ICurrentUserAppService
    {
        private readonly ISysUserRepository _userRep;

        private readonly ICurrentUser _currentUser;

        private readonly H_AppSettings _appSettings;


        public CurrentUserAppService(ISysUserRepository userRepository, ICurrentUser currentUser,
            IOptionsSnapshot<H_AppSettings> appSettingsOptions)
        {
            _userRep = userRepository;
            _currentUser = currentUser;
            _appSettings = appSettingsOptions.Value;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUserVM> Get()
        {
            var user = await _userRep.GetAsync(_currentUser.Id.Value);
            return user.Adapt<CurrentUserVM>();
        }

        // /// <summary>
        // /// 更新头像地址 (ImageSharp)
        // /// </summary>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // public async Task UpdateHeadImg(UpdateHeadImgRequest request)
        // {
        //     string[] str = request.Base64Str.Split(','); //base64Str为base64完整的字符串，先处理一下得到我们所需要的字符串
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
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateHeadImg(UpdateHeadImgRequest request)
        {
            var user = await _userRep.GetAsync(_currentUser.Id.Value);
            user.HeadImgUrl = $"https://{request.HeadImageUrl}";
            await _userRep.UpdateAsync(user, user => new { user.HeadImgUrl });
        }

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateBaseInfo(CurrentUserUpdateRequest vm)
        {
            var user = await _userRep.GetAsync(_currentUser.Id.Value);
            user.Phone = vm.Phone;
            user.WeChat = vm.WeChat;
            user.Profile = vm.Profile;
            user.HomeAddress = vm.HomeAddress;
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
            var user = await _userRep.GetAsync(_currentUser.Id.Value);
            oldPassword = H_EncryptProvider.HMACSHA256(oldPassword, _appSettings.Key.Sha256Key);

            H_AssertEx.That(user.Password != oldPassword, "原密码错误");

            user.PasswordLevel = (PasswordLevel)H_Util.CheckPasswordLevel(newPassword);
            newPassword = H_EncryptProvider.HMACSHA256(newPassword, _appSettings.Key.Sha256Key);
            user.Password = newPassword;
            await _userRep.UpdateAsync(user, user => new { user.Password, user.PasswordLevel });
        }

        /// <summary>
        /// 当前用户安全信息
        /// </summary>
        public async Task<UserSecurityVM> GetSecurityInfo()
        {
            var user = await _userRep.GetAsync(_currentUser.Id.Value);
            var result = user.Adapt<UserSecurityVM>();
            return result;
        }


        /// <summary>
        /// 注销当前登录
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            var key = $"{_appSettings.RedisPrefix.Login}{_currentUser.Id.Value}_{_currentUser.Jti}";

            var value = RedisHelper.Get(key);

            if (value.HasValue()) RedisHelper.Del(key);
        }
    }
}