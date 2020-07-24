using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
using Hao.File;
using Hao.Json;
using Hao.Library;
using Hao.Repository;
using Hao.RunTimeException;
using Hao.Utility;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 当前用户应用服务
    /// </summary>
    public class CurrentUserAppService : ApplicationService, ICurrentUserAppService
    {
        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;

        private readonly ICurrentUser _currentUser;

        private readonly H_AppSettingsConfig _appsettings;


        public CurrentUserAppService(ISysUserRepository userRepository, IMapper mapper, ICurrentUser currentUser,
            IOptionsSnapshot<H_AppSettingsConfig> appsettingsOptions)
        {
            _userRep = userRepository;
            _mapper = mapper;
            _currentUser = currentUser;
            _appsettings = appsettingsOptions.Value;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUserVM> GetUser()
        {
            var user = await _userRep.GetAysnc(_currentUser.Id.Value);
            return _mapper.Map<CurrentUserVM>(user);
        }

        /// <summary>
        /// 更新头像地址 (ImageSharp)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateHeadImg(UpdateHeadImgRequest request)
        {
            string[] str = request.Base64Str.Split(','); //base64Str为base64完整的字符串，先处理一下得到我们所需要的字符串
            if (str.Length < 2) throw new H_Exception("图片格式不对");
            byte[] imageBytes = Convert.FromBase64String(str[1]);

            H_File.CreateDirectory(_appsettings.FilePath.AvatarPath);
            string imgName = $"{_currentUser.Id}_{H_Util.GetUnixTimestamp()}.png";
            string imgPath = Path.Combine(_appsettings.FilePath.AvatarPath, imgName);
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(imageBytes))
            {
                image.Save(imgPath);
            }

            var user = await _userRep.GetAysnc(_currentUser.Id.Value);
            string oldImgUrl = user.HeadImgUrl;
            user.HeadImgUrl = imgName;
            await _userRep.UpdateAsync(user, user => new { user.HeadImgUrl });
            if (!string.IsNullOrWhiteSpace(oldImgUrl))
            {
                H_File.DeleteFile(Path.Combine(_appsettings.FilePath.AvatarPath, oldImgUrl));
            }
        }

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateBaseInfo(CurrentUserUpdateRequest vm)
        {
            var user = await _userRep.GetAysnc(_currentUser.Id.Value);
            user.Name = vm.Name;
            user.Age = vm.Age;
            user.Gender = vm.Gender;
            user.NickName = vm.NickName;
            user.Profile = vm.Profile;
            user.HomeAddress = vm.HomeAddress;
            user.FirstNameSpell = H_Spell.GetFirstLetter(user.Name.ToCharArray()[0]);
            await _userRep.UpdateAsync(user,
                user => new
                {
                    user.Name,
                    user.Age,
                    user.Gender,
                    user.NickName,
                    user.Profile,
                    user.HomeAddress,
                    user.FirstNameSpell
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
            var user = await _userRep.GetAysnc(_currentUser.Id.Value);
            oldPassword = EncryptProvider.HMACSHA256(oldPassword, _appsettings.Key.Sha256Key);
            if (user.Password != oldPassword) throw new H_Exception("原密码错误");
            user.PasswordLevel = (PasswordLevel)H_Util.CheckPasswordLevel(newPassword);
            newPassword = EncryptProvider.HMACSHA256(newPassword, _appsettings.Key.Sha256Key);
            user.Password = newPassword;
            await _userRep.UpdateAsync(user, user => new { user.Password, user.PasswordLevel });
        }

        /// <summary>
        /// 当前用户安全信息
        /// </summary>
        public async Task<UserSecurityVM> GetSecurityInfo()
        {
            var user = await _userRep.GetAysnc(_currentUser.Id.Value);
            var result = _mapper.Map<UserSecurityVM>(user);
            return result;
        }


        /// <summary>
        /// 注销当前登录
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            var key = $"{_appsettings.RedisPrefix.Login}{_currentUser.Id.Value}_{_currentUser.Jti}";

            var value = await RedisHelper.GetAsync(key);

            if (value.HasValue())
            {
                var cacheUser = H_JsonSerializer.Deserialize<H_RedisCacheUser>(value);
                cacheUser.LoginStatus = LoginStatus.Offline;

                await RedisHelper.SetAsync(key, H_JsonSerializer.Serialize(cacheUser));
            }
        }
    }
}