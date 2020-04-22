using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
using Hao.File;
using Hao.Library;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using Hao.Utility;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class UserAppService : ApplicationService, IUserAppService
    {

        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;


        private readonly ISysLoginRecordRepository _recordRep;


        private readonly ISysRoleRepository _roleRep;

        private readonly AppSettingsInfo _appsettings;

        public FilePathInfo PathInfo { get; set; }

        private readonly ICurrentUser _currentUser;

        public UserAppService(ISysRoleRepository roleRep,
            IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, 
            ISysUserRepository userRepository,
            ISysLoginRecordRepository recordRep, 
            IMapper mapper,
            ICurrentUser currentUser)
        {
            _userRep = userRepository;
            _recordRep = recordRep;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value;
            _currentUser = currentUser;
            _roleRep = roleRep;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task AddUser(UserAddRequest vm)
        {
            var users = await _userRep.GetListAysnc(new UserQuery()
            {
                LoginName = vm.LoginName
            });
            if (users.Count > 0) throw new HException("账号已存在，请重新输入");

            var role = await _roleRep.GetAysnc(vm.RoleId.Value);
            if (role == null) throw new HException("角色不存在，请重新添加");
            if (role.IsDeleted) throw new HException("角色已删除，请重新添加");

            var user = _mapper.Map<SysUser>(vm);
            user.FirstNameSpell = HSpell.GetFirstLetter(user.Name.ToCharArray()[0]);
            user.PasswordLevel = (PasswordLevel)HUtil.CheckPasswordLevel(user.Password);
            user.Password = EncryptProvider.HMACSHA256(user.Password, _appsettings.KeyInfo.Sha256Key);
            user.Enabled = true;
            user.RoleId = role.Id;
            user.RoleName = role.Name;
            user.AuthNumbers = role.AuthNumbers;
            await _userRep.InsertAysnc(user);
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vms"></param>
        /// <returns></returns>
        public async Task AddUsers(List<UserAddRequest> vms)
        {
            var users = _mapper.Map<List<SysUser>>(vms);
            foreach (var user in users)
            {
                user.FirstNameSpell = HSpell.GetFirstLetter(user.Name.ToCharArray()[0]);
                user.Password = EncryptProvider.HMACSHA256(user.Password, _appsettings.KeyInfo.Sha256Key);
            }
            await _userRep.InsertAysnc(users);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<UserItemVM>> GetUserPageList(UserQuery query)
        {
            var users = await _userRep.GetPagedListAysnc(query);
            var result = _mapper.Map<PagedList<UserItemVM>>(users);

            return result;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDetailVM> GetUser(long id)
        {
            var user = await GetUserDetail(id);
            return _mapper.Map<UserDetailVM>(user);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUserVM> GetCurrent()
        {
            var user = await GetUserDetail(_currentUser.Id);
            return _mapper.Map<CurrentUserVM>(user);
        }


        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastLoginTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>

        [UnitOfWork]
        public async Task UpdateLogin(long userId, DateTime lastLoginTime, string ip)
        {
            var user = await GetUserDetail(userId);
            user.LastLoginTime = lastLoginTime;
            user.LastLoginIP = ip;

            await _userRep.UpdateAsync(user, user => new { user.LastLoginTime, user.LastLoginIP });
            await _recordRep.InsertAysnc(new SysLoginRecord() { UserId = user.Id, IP = user.LastLoginIP, Time = user.LastLoginTime });
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUser(long userId)
        {
            if (userId == _currentUser.Id) throw new HException("无法操作当前登录用户");
            if (userId == -1) throw new HException("无法操作系统管理员账户");
            await _userRep.DeleteAysnc(userId);
            await RedisHelper.DelAsync(_appsettings.RedisPrefixOptions.LoginInfo + userId);
        }

        /// <summary>
        /// 注销/启用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task UpdateUserStatus(long userId, bool enabled)
        {
            if (userId == _currentUser.Id) throw new HException("无法操作当前登录用户");
            if (userId == -1) throw new HException("无法操作系统管理员账户");
            var user = await GetUserDetail(userId);
            user.Enabled = enabled;
            await _userRep.UpdateAsync(user, user => new { user.Enabled });
            if(!enabled) await RedisHelper.DelAsync(_appsettings.RedisPrefixOptions.LoginInfo + user.Id);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task EditUser(long userId, UserUpdateRequest vm)
        {
            if (userId == -1) throw new HException("无法操作系统管理员账户");
            var user = await GetUserDetail(userId);
            user.Name = vm.Name;
            user.Age = vm.Age;
            user.Gender = vm.Gender;
            user.Phone = vm.Phone;
            user.Email = vm.Email;
            user.WeChat = vm.WeChat;
            user.QQ = vm.QQ;
            await _userRep.UpdateAsync(user,
                user => new { user.Name, user.Age, user.Gender, user.Phone, user.Email, user.WeChat, user.QQ, user.RoleId, user.RoleName });
        }

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistUser(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query);
            return users.Count > 0;
        }


        /// <summary>
        /// 更新头像地址 (ImageSharp)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateCurrentHeadImg(UpdateHeadImgRequest request)
        {
            string[] str = request.Base64Str.Split(',');  //base64Str为base64完整的字符串，先处理一下得到我们所需要的字符串
            byte[] imageBytes = Convert.FromBase64String(str[1]);

            HFile.CreateDirectory(PathInfo.AvatarPath);
            string imgName = $"{_currentUser.Id}_{HUtil.GetTimeStamp()}.png";
            string imgPath = Path.Combine(PathInfo.AvatarPath, imgName);
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(imageBytes)) 
            {
                image.Save(imgPath);
            }  
            var user = await GetUserDetail(_currentUser.Id);
            string oldImgUrl = user.HeadImgUrl;
            user.HeadImgUrl = imgName;
            await _userRep.UpdateAsync(user, user => new { user.HeadImgUrl });
            if (!string.IsNullOrWhiteSpace(oldImgUrl))
            {
                HFile.DeleteFile(Path.Combine(PathInfo.AvatarPath, oldImgUrl));
            }

        }

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateCurrentBaseInfo(CurrentUserUpdateRequest vm)
        {
            var user = await GetUserDetail(_currentUser.Id);
            user.Name = vm.Name;
            user.Age = vm.Age;
            user.Gender = vm.Gender;
            user.NickName = vm.NickName;
            user.Profile = vm.Profile;
            user.HomeAddress = vm.HomeAddress;
            user.FirstNameSpell = HSpell.GetFirstLetter(user.Name.ToCharArray()[0]);
            await _userRep.UpdateAsync(user,
                user => new { user.Name, user.Age, user.Gender, user.NickName, user.Profile, user.HomeAddress,user.FirstNameSpell });
        }

        /// <summary>
        /// 更新当前用户密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task UpdateCurrentPassword(string oldPassword, string newPassword)
        {
            var user = await GetUserDetail(_currentUser.Id);
            oldPassword = EncryptProvider.HMACSHA256(oldPassword, _appsettings.KeyInfo.Sha256Key);
            if (user.Password != oldPassword) throw new HException("原密码错误");
            user.PasswordLevel = (PasswordLevel)HUtil.CheckPasswordLevel(newPassword);
            newPassword = EncryptProvider.HMACSHA256(newPassword, _appsettings.KeyInfo.Sha256Key);
            user.Password = newPassword;
            await _userRep.UpdateAsync(user, user => new {user.Password, user.PasswordLevel});
        }

        /// <summary>
        /// 当前用户安全信息
        /// </summary>
        public async Task<UserSecurityVM> GetCurrentSecurityInfo()
        {
            var user = await GetUserDetail(_currentUser.Id);
            var result =  _mapper.Map<UserSecurityVM>(user);
            return result;
        }

        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> ExportUsers(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query);

            var exportData = users.Select(a => new Dictionary<string, string>{
                {"姓名",a.Name},
                {"性别", a.Gender.GetDescription() },
                {"年龄",a.Age.ToString()},
                {"手机号",a.Phone},
                {"邮箱",a.Email},
                {"微信",a.WeChat},
                {"状态",a.Enabled.IsTrue()?"启用":"注销"},
                {"最后登录时间",a.LastLoginTime.ToDateString()},
                {"最后登录地点",a.LastLoginIP}
            });

            string fileName = $"{Guid.NewGuid()}.xlsx";
            string rootPath = PathInfo.ExportExcelPath;

            HFile.CreateDirectory(rootPath);
            string filePath = Path.Combine(rootPath, $"{fileName}");

            await HFile.ExportToExcelEPPlus(filePath, exportData);

            return fileName;
        }



        #region private

        private async Task<SysUser> GetUserDetail(long userId)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user == null) throw new HException("用户不存在");
            if (user.IsDeleted) throw new HException("用户已删除");
            return user;
        }


        #endregion
    }
}
